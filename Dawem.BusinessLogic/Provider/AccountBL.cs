using Dawem.Contract.BusinessLogic.Others;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
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
using Dawem.Models.Response.Identity;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
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
        private readonly IUserRepository userRepository;
        private readonly IBranchBL branchBL;
        private readonly ICompanyBL companyBL;

        private readonly Jwt jwt;
        private readonly RequestHeaderContext userContext;
        private readonly IMailBL mailBL;
        private readonly IHttpContextAccessor accessor;
        private readonly LinkGenerator generator;
        private readonly IUserTokenRepository userTokenRepository;
        private readonly IRegisterationBLValidation registerationValidatorBL;
        private readonly IUserBranchRepository userBranchRepository;
        private readonly IBranchBLValidation branchValidatorBL;
        private readonly IBranchRepository branchRepository;
        private readonly IActionLogBL actionLogBL;
        public AccountBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IBranchRepository _branchRepository, IActionLogBL _actionLogBL,
            IBranchBLValidation _branchValidatorBL, IUserRepository _userRepository,
            UserManagerRepository _userManagerRepository,
           IBranchBL _branchBL, IOptions<Jwt> _appSettings,
           ICompanyBL _companyBL, RequestHeaderContext _userContext,
            IMailBL _mailBL,
           IUserTokenRepository _userTokenRepository, IHttpContextAccessor _accessor,
           LinkGenerator _generator, IRegisterationBLValidation _registerationValidatorBL,
           IUserBranchRepository _userBranchRepository)
        {
            unitOfWork = _unitOfWork;
            actionLogBL = _actionLogBL;
            userManagerRepository = _userManagerRepository;
            userRepository = _userRepository;
            branchBL = _branchBL;
            companyBL = _companyBL;
            branchRepository = _branchRepository;
            userContext = _userContext;
            jwt = _appSettings.Value;
            branchValidatorBL = _branchValidatorBL;
            mailBL = _mailBL;
            accessor = _accessor;
            generator = _generator;
            userTokenRepository = _userTokenRepository;
            registerationValidatorBL = _registerationValidatorBL;
            userBranchRepository = _userBranchRepository;
        }
        private async Task<User> CreateUser(RegisterModel model)
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
                throw new BusinessValidationErrorException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }

            var assignRole = await userManagerRepository.AddToRoleAsync(user, RoleName);
            if (!assignRole.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationErrorException(DawemKeys.SorryErrorHappenWhileAddingUser);
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
            User user = new();

            #region Get User Using AccessToken

            var Email = signInModel.UserName;
            if (!string.IsNullOrEmpty(Email))
            {
                user = await userManagerRepository.FindByEmailAsync(Email);
                if (user == null)
                {
                    throw new BusinessValidationErrorException(DawemKeys.SorryUserNotFound);
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
                throw new BusinessValidationErrorException(DawemKeys.SorryPasswordIncorrectEnterCorrectPasswordForSelectedUser);
            }

            #endregion

            if (!user.EmailConfirmed)
            {
                throw new BusinessValidationErrorException(DawemKeys.SorryEmailNotConfirmedPleaseCheckYourEmail);
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

            var getTokenModelResult = GetTokenModel(tokenModelSearchCriteria);

            if (getTokenModelResult.Status != ResponseStatus.Success)
            {
                TranslationHelper.MapBaseResponse(getTokenModelResult, response);
                return response;
            }


            userContext.UserId = user.Id;
            userContext.BranchId = signInModel.BranchId;

            #endregion

            response.TokeObject = getTokenModelResult.TokenData;


            response.Status = ResponseStatus.Success;
            return response;
        }

        public TokenModelSearchResult GetTokenModel(TokenModelSearchCriteria criteria)
        {

            #region Create Token

            ClaimsIdentity claimsIdentity = new(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, criteria.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, criteria.UserId.ToString()),
                new Claim(DawemKeys.UserId, criteria.UserId.ToString()),
                new Claim(DawemKeys.BranchId , criteria.BranchId.ToString())
            });
            if (criteria.RememberMe)
            {
                claimsIdentity.AddClaim(new Claim("rememberMe", "true"));
            }
            if (criteria.Roles != null)
            {
                claimsIdentity.AddClaims(criteria.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            JwtSecurityTokenHandler tokenHandler = new();

            var key = Encoding.ASCII.GetBytes(jwt.Key);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = claimsIdentity,
                Expires = criteria.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


            string Token = tokenHandler.WriteToken(token);

            FormateTokenSearchResult tokenModelResult = FormateToken(criteria.UserId, criteria.BranchId, Token);

            if (tokenModelResult.Status != ResponseStatus.Success)
            {
                TranslationHelper.MapBaseResponse(tokenModelResult, response);
                return response;
            }



            response.TokenData = tokenModelResult.TokenData;
            response.AuthToken = Token;


            #endregion


            return response;
        }


        public FormateTokenSearchResult FormateToken(int? UserId, int BranchId, string Token)
        {

            FormateTokenSearchResult result = new()
            {
                Status = ResponseStatus.Success
            };

            var branch = branchRepository.Get(a => a.Id == BranchId).Include(a => a.Company).FirstOrDefault();


            if (branch == null)
            {
                TranslationHelper.SetSearchResultMessages(result, "BranchNotFound", "Sorry! Branch Not Found  !", userContext.Lang ?? "ar");

                result.Status = ResponseStatus.ValidationError;
                return result;
            }
            UserDTOMapper.InitUserContext(userContext);
            UserDTO user = UserDTOMapper.Map(userRepository.GetByID(UserId));


            #region Get Token For Others

            TokenDto tokenModel = new()
            {
                UserId = user.Id,
                BranchName = branch.BranchName,
                UserFullName = user.FirstName + " " + user.LastName,
                Token = Token,
                UserName = user.UserName,
                CurrentBranchId = branch.Id,
                CompanyId = branch.CompanyId,
                CompanyName = branch.Company?.CompanyName,
                IsMainBranch = branch.IsMainBranch

            };


            result.TokenData = tokenModel;

            return result;

            #endregion

        }


        public async Task<BaseResponseT<RegisterResponseModel>> RegisterBasic(RegisterModel model)
        {
            var response = new BaseResponseT<RegisterResponseModel>();
            User user = null;

            string RoleName = "FullAccess";

            userContext.IsMainBranch = true;
            if (model.FirstName == null || model.LastName == null)
            {
                TranslationHelper.SetValidationMessages(response, "MissingData", "Missing Name", userContext.Lang);
                return response;
            }
            try
            {

                var resValid = await registerationValidatorBL.RegisterationValidator(model);
                if (resValid.Status != ResponseStatus.Success)
                {
                    return resValid;
                }
                model.UserMobileNumber = MobileHelper.HandleMobile(model.UserMobileNumber);
                unitOfWork.CreateTransaction();


                #region insert user





                var userRes = await CreateUser(model);
                if (userRes.Status != ResponseStatus.Success)
                {
                    response.Status = userRes.Status;
                    response.Message = userRes.Message;
                    return response;
                }
                user = userRes.Result;
                #endregion

                #region insert company



                int companyId;

                var company = new Company()
                {
                    CompanyName = model.BusinessName,

                    IsActive = true,

                    CountryId = model.BusinessCountryId

                };


                var companyResponse = companyBL.Create(company);

                if (companyResponse.Status != ResponseStatus.Success)
                {
                    unitOfWork.Rollback();

                    TranslationHelper.MapBaseResponse(companyResponse, response);
                    return response;
                }
                companyId = company.Id;


                #endregion

                #region insert account Branch

                #region Set Default Package
                var pack = model.PackageId != null ? packageRepository
                    .GetByID(model.PackageId) : null;
                if (pack == null)
                {
                    model.PackageId = null;
                }
                if (model.PackageId == null)
                {
                    var getDefaultPackage = packageRepository.GetEntityByCondition(p => p.IsDefaultPackage);

                    if (getDefaultPackage != null)
                    {
                        model.PackageId = getDefaultPackage.Id;
                    }
                }

                #endregion

                Branch branch = new Branch()
                {
                    CompanyId = companyId,

                    Email = model.BusinessEmail,

                    CurrencyId = model.BusinessCurrencyId,
                    IsActive = true,
                    AdminUserId = user.Id,
                    BranchName = model.BusinessName,
                    CommercialRecordNumber = model.BusinessCommercialRecordNumber,
                    TaxRegistrationNumber = model.BusinessTaxRegistrationNumber,
                    IsMainBranch = true,

                    PackageId = model.PackageId,
                    CountryId = model.BusinessCountryId,

                    Address = model.BusinessAddress,


                    financialYears = new List<FinancialYear>
                    {
                        new FinancialYear
                        {
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddYears(1)
                        }
                    },




                };
                BranchDTOMapper.InitBranchContext(userContext);
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
                    CurrencyId = model.BusinessCurrencyId,
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
                var registerResponseModel = new RegisterResponseModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    branchId = branch.Id,
                    CompanyId = Convert.ToInt32(branch.CompanyId),


                };

                response.Result = registerResponseModel;


            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    await userManagerRepository.RemoveFromRoleAsync(user, RoleName);
                    await userManagerRepository.DeleteAsync(user);
                }
                TranslationHelper.SetException(response, ex);
            }
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
                        TranslationHelper.MapIdentityResultToBaseResponse(response, result, userContext.Lang);
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
                            Subject = userContext.Lang == "ar" ? "مرحبًا بك في سمارت بيزنس! أعد تعيين كلمة المرور الخاصة بك" : "Welcome to  Business! Reset Your Password",
                            UserName = user.FirstName + " " + user.LastName,
                            Body = string.Format("{0} <a href=\"" + confirmationLink + "\">here</a>",
                            userContext.Lang == "ar" ? " يرجى إعادة تعيين كلمة المرور الخاصة بك عن طريق النقر علي" : "Please reset your password by clicking"),
                            Email = user.Email

                        };

                        var MailResponse = await mailBL.SendEmail(verifyEmail);

                    }
                    catch (Exception ex)
                    {

                        TranslationHelper.SetValidationMessages(response, "NotValidEmail", userContext.Lang == "ar" ? "بريدك الالكتروني غير صالح" : "Not valid email", "", lang: userContext.Lang);
                    }
                    await unitOfWork.SaveAsync();
                    unitOfWork.Commit();
                    response.Status = ResponseStatus.Success;

                    TranslationHelper.SetResponseMessages(response, "", userContext.Lang == "ar" ? "الرجاء تفحص بريدك الإلكتروني ! أرسلنا لك البريد لإعادة تعيين كلمة المرور الخاصة بك" :
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



        public async Task<BaseResponseT<RegisterResponseModel>> ChangePassword(ChangePasswordBindingModel model)
        {
            BaseResponseT<RegisterResponseModel> response = new BaseResponseT<RegisterResponseModel>();
            RegisterResponseModel _RegisterResponseModel = new RegisterResponseModel();

            try
            {
                var user = await userManagerRepository.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    TranslationHelper.SetValidationMessages(response, "NoUserWithSuchName", "NoUserWithSuchName", lang: userContext.Lang);
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
                _RegisterResponseModel.UserName = user.UserName;
                response.Result = _RegisterResponseModel;
                response.Status = ResponseStatus.Success;
                TranslationHelper.SetResponseMessages(response, "ChangedPass", userContext.Lang == "ar" ? "تم تغيير كلمة المرور" : "Password has been changed !", lang: userContext.Lang);

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

