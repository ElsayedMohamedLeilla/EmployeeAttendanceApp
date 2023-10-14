using Dawem.Contract.BusinessLogic.Others;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Provider;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.DtosMappers;
using Dawem.Models.Exceptions;
using Dawem.Models.Generic;
using Dawem.Models.Response;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Dawem.Validation.FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dawem.BusinessLogic.Provider
{
    public class AccountBL : IAccountBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly UserManagerRepository userManagerRepository;
        private readonly IBranchBL branchBL;
        private readonly ICompanyBL companyBL;
        private readonly Jwt jwt;
        private readonly RequestHeaderContext requestHeaderContext;
        private readonly IMailBL mailBL;
        private readonly IHttpContextAccessor accessor;
        private readonly LinkGenerator generator;
        private readonly IRegisterationBLValidation registerationValidatorBL;
        private readonly IBranchBLValidation branchValidatorBL;
        private readonly IRepositoryManager repositoryManager;
        private readonly IActionLogBL actionLogBL;
        public AccountBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
           IBranchBLValidation _branchValidatorBL,
            IActionLogBL _actionLogBL,
            IRepositoryManager _repositoryManager,
            UserManagerRepository _userManagerRepository,
           IBranchBL _branchBL, IOptions<Jwt> _appSettings,
           ICompanyBL _companyBL, RequestHeaderContext _userContext,
            IMailBL _mailBL, IHttpContextAccessor _accessor,
           LinkGenerator _generator, IRegisterationBLValidation _registerationValidatorBL)
        {
            unitOfWork = _unitOfWork;
            actionLogBL = _actionLogBL;
            userManagerRepository = _userManagerRepository;
            branchBL = _branchBL;
            companyBL = _companyBL;
            requestHeaderContext = _userContext;
            jwt = _appSettings.Value;
            repositoryManager = _repositoryManager;
            branchValidatorBL = _branchValidatorBL;
            mailBL = _mailBL;
            accessor = _accessor;
            generator = _generator;
            registerationValidatorBL = _registerationValidatorBL;
        }
        private async Task<User> CreateUser(SignUpModel model)
        {
            await unitOfWork.CreateTransactionAsync();

            string RoleName = DawemKeys.FullAccess;
            var user = new User()
            {
                UserName = model.UserEmail,
                Email = model.UserEmail,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MobileNumber = model.UserMobileNumber,
                IsAdmin = true,
                IsActive = true
            };

            var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                //var errors1= string.Join(DawemKeys.Comma, createUserResponse.Errors.Select(x => x.Description).FirstOrDefault());
                //var errors2 = createUserResponse.Errors.Select(x => x.Code).FirstOrDefault();
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }

            var assignRole = await userManagerRepository.AddToRoleAsync(user, RoleName);
            if (!assignRole.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }

            return user;
        }
        private string GenerateConfirmEmailLink(object emailToken)
        {

            var path = generator.GetPathByAction("VerifyEmail", "Account", emailToken);
            var protocol = accessor.HttpContext.Request.IsHttps ? "https" : "http";
            var host = accessor.HttpContext.Request.Host.Value;
            var confirmEmailLink = $"{protocol}://{host}{path}";
            return confirmEmailLink;
        }
        public async Task<TokenDto> SignIn(SignInModel signInModel)
        {

            #region Model Validation

            var signInModelValidator = new SignInModelValidator();
            var signInModelValidatorResult = signInModelValidator.Validate(signInModel);
            if (!signInModelValidatorResult.IsValid)
            {
                var error = signInModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Get User Using AccessToken

            User user = new();
            var Email = signInModel.Email;
            if (!string.IsNullOrEmpty(Email))
            {
                user = await userManagerRepository.FindByEmailAsync(Email);
                if (user == null)
                {
                    throw new BusinessValidationException(DawemKeys.SorryUserNotFound);
                }
            }

            #endregion

            #region Handle User Role

            var roles = await userManagerRepository.GetRolesAsync(user);
            if (roles.FirstOrDefault(r => r == DawemKeys.FullAccess) == null)
            {
                var addingToRoleResult = await userManagerRepository.AddToRoleAsync(user, DawemKeys.FullAccess);

                if (addingToRoleResult.Succeeded)
                {
                    roles.Add(DawemKeys.FullAccess);
                }
            }

            #endregion

            #region Check Password

            bool checkPasswordAsyncRes = await userManagerRepository.CheckPasswordAsync(user, signInModel.Password);
            if (!checkPasswordAsyncRes)
            {
                throw new BusinessValidationException(DawemKeys.SorryPasswordIncorrectEnterCorrectPasswordForSelectedUser);
            }

            #endregion

            if (!user.EmailConfirmed)
            {
                throw new BusinessValidationException(DawemKeys.SorryEmailNotConfirmedPleaseCheckYourEmail);
            }

            #region Get And Validate Branch

            var validateUserBranchSearchCriteria = new ValidateUserBranchSearchCriteria
            {
                UserId = user.Id,
                UserName = user.UserName,
                BranchId = signInModel.BranchId
            };

            await branchValidatorBL.ValidateUserBranch(validateUserBranchSearchCriteria);

            #endregion

            #region Get Token Model

            TokenModelSearchCriteria tokenModelSearchCriteria = new()
            {
                BranchId = signInModel.BranchId,
                UserId = user.Id,
                UserName = user.UserName,
                RememberMe = signInModel.RememberMe,
                Roles = roles
            };

            var tokenData = await GetTokenModel(tokenModelSearchCriteria);

            #endregion

            return tokenData;
        }
        public async Task<TokenDto> GetTokenModel(TokenModelSearchCriteria criteria)
        {

            #region Create Token

            ClaimsIdentity claimsIdentity = new(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, criteria.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, criteria.UserId.ToString()),
                new Claim(DawemKeys.UserId, criteria.UserId.ToString()),
                new Claim(DawemKeys.BranchId , criteria.BranchId.ToString()),
                new Claim(DawemKeys.ApplicationType , criteria.ApplicationType.ToString())
            });
            if (criteria.RememberMe)
            {
                claimsIdentity.AddClaim(new Claim(DawemKeys.RememberMe, DawemKeys.True));
            }
            if (criteria.Roles != null)
            {
                claimsIdentity.AddClaims(criteria.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            JwtSecurityTokenHandler tokenHandler = new();

            var key = Encoding.ASCII.GetBytes(jwt.Key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = criteria.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            var tokenData = await FormateToken(criteria.UserId, criteria.BranchId, token);

            #endregion

            return tokenData;
        }
        public async Task<TokenDto> FormateToken(int? userId, int branchId, string token)
        {
            var branchRepository = repositoryManager.BranchRepository;
            var branch = await branchRepository.Get(a => a.Id == branchId)
                .Include(a => a.Company).FirstOrDefaultAsync();

            if (branch == null)
            {
                throw new BusinessValidationException(DawemKeys.SorryBranchNotFound);
            }
            UserDTOMapper.InitUserContext(requestHeaderContext);
            UserDTO user = UserDTOMapper.Map(await repositoryManager.UserRepository.GetByIdAsync(userId));


            #region Get Token

            TokenDto tokenModel = new()
            {
                UserId = user.Id,
                BranchName = branch.BranchName,
                UserFullName = user.FirstName + DawemKeys.Space + user.LastName,
                Token = token,
                Email = user.UserName,
                CurrentBranchId = branch.Id,
                CompanyId = branch.CompanyId,
                CompanyName = branch.Company?.CompanyName,
                IsMainBranch = branch.IsMainBranch
            };

            #endregion

            return tokenModel;
        }
        public async Task<SignUpResponseModel> SignUp(SignUpModel signUpModel)
        {
            #region Model Validation

            var signUpModelValidator = new SignUpModelValidator();
            var signUpModelValidatorResult = signUpModelValidator.Validate(signUpModel);
            if (!signUpModelValidatorResult.IsValid)
            {
                var error = signUpModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            string? RoleName = DawemKeys.FullAccess;
            requestHeaderContext.IsMainBranch = true;

            registerationValidatorBL.RegisterationValidator(signUpModel);

            signUpModel.UserMobileNumber = MobileHelper.HandleMobile(signUpModel.UserMobileNumber);

            unitOfWork.CreateTransaction();


            #region insert user

            var user = await CreateUser(signUpModel);

            #endregion

            #region Insert Company

            var insertedCompany = repositoryManager.CompanyRepository.Insert(new Company()
            {
                CompanyName = signUpModel.CompanyName,
                IsActive = true,
                CountryId = signUpModel.CompanyCountryId
            });
            await unitOfWork.SaveAsync();
            var companyId = insertedCompany.Id;

            #endregion

            #region insert account Branch

            Branch branch = new Branch()
            {
                CompanyId = companyId,
                Email = signUpModel.CompanyEmail,
                CurrencyId = signUpModel.BusinessCurrencyId,
                IsActive = true,
                AdminUserId = user.Id,
                BranchName = signUpModel.CompanyName,
                CommercialRecordNumber = signUpModel.BusinessCommercialRecordNumber,
                TaxRegistrationNumber = signUpModel.BusinessTaxRegistrationNumber,
                IsMainBranch = true,
                PackageId = signUpModel.PackageId,
                CountryId = signUpModel.CompanyCountryId,
                Address = signUpModel.CompanyAddress,
            };
            BranchDTOMapper.InitBranchContext(requestHeaderContext);
            var BranchDTO = BranchDTOMapper.Map(branch);
            var branchResponse = await branchBL.Create(BranchDTO);
            if (branchResponse.Status != ResponseStatus.Success)
            {
                unitOfWork.Rollback();
                TranslationHelper.MapBaseResponse(branchResponse, response);
                return response;
            }

            #region Update User Main Branch

            var getUser = userRepository.GetByID(user.Id);
            getUser.MainBranchId = branchResponse?.Result;
            unitOfWork.Save();

            #endregion

            #endregion

            #region insert user branch


            var userBranches = new UserBranch()
            {

                UserId = user.Id,
                BranchId = branchResponse.Result
            };

            try
            {
                var createuserBranchesResponse = userBranchRepository.Insert(userBranches);

                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex);
            }

            #endregion

            #region insert currency

            var accountCurrency = new BranchCurrency()
            {

                BranchId = branchResponse.Result,
                CurrencyId = signUpModel.BusinessCurrencyId,
                CurrencyFactor = 1,
                IsActive = true,
                IsDefault = true

            };
            try
            {
                var createCurrencyResponse = branchCurrencyRepository.Insert(accountCurrency);

                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex);
            }
            #endregion



            //Add Token to verify Email 
            var token = await userManagerRepository.GenerateEmailConfirmationTokenAsync(user);
            var emailToken = new { emailtoken = token, email = user.Email };


            var confirmationLink = GenerateConfirmEmailLink(emailToken);
            VerifyEmailModel verifyEmail = new VerifyEmailModel
            {
                Email = user.Email,
                Subject = "شكرا للتسجيل مع سمارت بيزنس",
                Body = @"<meta charset='UTF-8'>
                                            <title>شكرا للتسجيل مع سمارت بيزنس</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>مرحباً  " + user.FirstName + " " + user.LastName + @"</h1>
                                            <p>شكراً لتسجيلك بالتجربة المجانية مع شركة سمارت بيزنس.</p>
                                            <p>لاستكمال عملية التسجيل، يرجى الضغط على الرابط التالي لتأكيد عنوان البريد الإلكتروني الخاص بك وتفعيل الحساب الجديد:</p>
                                            <p><a href=' " + confirmationLink + @" '> " + confirmationLink + @" </a></p>
                                            <p>فريق خدمة العملاء لشركة سمارت بيزنس يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:info@smart-bt.com'>info@smart-bt.com</a></p>
                                            <p> الهاتف: (+964)78000117
                                            </body>
                                            </html>"


            };
            try
            {

                var MailResponse = await mailBL.SendEmail(verifyEmail);

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex);
            }

            #region UserTokens

            var userToken = new UserToken()
            {

                UserId = user.Id,
                LoginProvider = "email",
                Name = "Email Confirmation Token",
                Value = token

            };

            try
            {
                var createCurrencyResponse = userTokenRepository.Insert(userToken);

                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex);
            }
            #endregion

            //try
            //{
            //    VerifyEmailModel verifyEmail = new VerifyEmailModel
            //    {
            //        Email = user.Email,
            //        UserName = user.FirstName + " " + user.LastName,
            //        verificationCode = EmailHelper.GenerateVerificationCode()
            //};

            //    var MailResponse = mailBL.SendEmail(verifyEmail);

            //  user.verificationCode = verifyEmail.verificationCode;
            //response.Status = ResponseStatus.Success;

            //}
            //catch (Exception ex)
            //{
            //    unitOfWork.Rollback();
            //    TranslationHelper.SetException(response, ex);
            //}


            unitOfWork.Save();
            unitOfWork.Commit();
            var registerResponseModel = new SignUpResponseModel()
            {
                UserId = user.Id,
                Email = user.UserName,
                BranchId = branch.Id,
                CompanyId = Convert.ToInt32(branch.CompanyId),


            };

            response.Result = registerResponseModel;



            return response;

        }
        public async Task<BaseResponseT<bool>> VerifyEmail(string token, string email)
        {
            var response = new BaseResponseT<bool>();
            try
            {
                var user = await userManagerRepository.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await userManagerRepository.ConfirmEmailAsync(user, token);
                    if (!result.Succeeded)
                    {
                        TranslationHelper.MapIdentityResultToBaseResponse(response, result, requestHeaderContext.Lang);
                        response.Result = false;
                        return response;

                    }
                    //else
                    //{
                    //  var userToken =  userTokenRepository.Get(a => a.Value == token && a.UserId == user.Id).FirstOrDefault();
                    //    if (userToken != null) 
                    //   userTokenRepository.Delete(userToken);
                    //   await unitOfWork.SaveAsync();
                    //}

                }
            }
            catch (Exception ex) { }
            response.Result = true;
            response.Status = ResponseStatus.Success;
            return response;
        }


        public async Task<BaseResponseT<bool>> ForgetPassword(ForgetPasswordBindingModel forgetPasswordBindingModel)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {

                var user = await userManagerRepository.FindByEmailAsync(forgetPasswordBindingModel.Email);
                if (user != null || await userManagerRepository.IsEmailConfirmedAsync(user))
                {

                    unitOfWork.CreateTransaction();

                    var token = await userManagerRepository.GenerateEmailConfirmationTokenAsync(user);
                    var emailToken = new { emailtoken = token, email = user.Email };

                    //#region UserTokens

                    //var userToken = new UserToken()
                    //{

                    //    UserId = user.Id,
                    //    LoginProvider = "ASP.NET Identity",
                    //    Name = "PasswordReset",
                    //    Value = token

                    //};

                    //try
                    //{
                    //    var createCurrencyResponse = userTokenRepository.Insert(userToken);

                    //    response.Status = ResponseStatus.Success;

                    //}
                    //catch (Exception ex)
                    //{
                    //    unitOfWork.Rollback();
                    //    TranslationHelper.SetException(response, ex);
                    //}
                    //#endregion

                    try
                    {
                        var confirmationLink = GenerateConfirmEmailLink(emailToken);
                        VerifyEmailModel verifyEmail = new VerifyEmailModel
                        {
                            Subject = requestHeaderContext.Lang == "ar" ? "مرحبًا بك في سمارت بيزنس! أعد تعيين كلمة المرور الخاصة بك" : "Welcome to  Business! Reset Your Password",
                            UserName = user.FirstName + " " + user.LastName,
                            Body = string.Format("{0} <a href=\"" + confirmationLink + "\">here</a>",
                            requestHeaderContext.Lang == "ar" ? " يرجى إعادة تعيين كلمة المرور الخاصة بك عن طريق النقر علي" : "Please reset your password by clicking"),
                            Email = user.Email

                        };

                        var MailResponse = await mailBL.SendEmail(verifyEmail);

                    }
                    catch (Exception ex)
                    {

                        TranslationHelper.SetValidationMessages(response, "NotValidEmail", requestHeaderContext.Lang == "ar" ? "بريدك الالكتروني غير صالح" : "Not valid email", "", lang: requestHeaderContext.Lang);
                    }
                    await unitOfWork.SaveAsync();
                    unitOfWork.Commit();
                    response.Status = ResponseStatus.Success;

                    TranslationHelper.SetResponseMessages(response, "", requestHeaderContext.Lang == "ar" ? "الرجاء تفحص بريدك الإلكتروني ! أرسلنا لك البريد لإعادة تعيين كلمة المرور الخاصة بك" :
                        "please check your mail ! we sent you mail to reset your password.");
                    return response;
                }


            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }

            return response;

        }



        public async Task<BaseResponseT<SignUpResponseModel>> ChangePassword(ChangePasswordBindingModel model)
        {
            BaseResponseT<SignUpResponseModel> response = new BaseResponseT<SignUpResponseModel>();
            SignUpResponseModel _RegisterResponseModel = new SignUpResponseModel();

            try
            {
                var user = await userManagerRepository.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    TranslationHelper.SetValidationMessages(response, "NoUserWithSuchName", "NoUserWithSuchName", lang: requestHeaderContext.Lang);
                    return response;
                }
                IdentityResult result = await userManagerRepository.ChangePasswordAsync(user, model.OldPassword,
                    model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        TranslationHelper.SetValidationMessages(response, "InCorrectPassword", error.Description);
                    }
                    response.Status = ResponseStatus.ValidationError;
                    return response;
                }

                _RegisterResponseModel.UserId = user.Id;
                _RegisterResponseModel.Email = user.UserName;
                response.Result = _RegisterResponseModel;
                response.Status = ResponseStatus.Success;
                TranslationHelper.SetResponseMessages(response, "ChangedPass", requestHeaderContext.Lang == "ar" ? "تم تغيير كلمة المرور" : "Password has been changed !", lang: requestHeaderContext.Lang);

            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
                return response;
            }
            return response;
        }

    }
}

