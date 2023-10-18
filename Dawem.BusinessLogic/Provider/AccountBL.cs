using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Provider;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.DtosMappers;
using Dawem.Models.Exceptions;
using Dawem.Models.Generic;
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
        private readonly Jwt jwt;
        private readonly RequestHeaderContext requestHeaderContext;
        private readonly IMailBL mailBL;
        private readonly IHttpContextAccessor accessor;
        private readonly LinkGenerator generator;
        private readonly IAccountBLValidation accountBLValidation;
        private readonly IBranchBLValidation branchValidatorBL;
        private readonly IRepositoryManager repositoryManager;
        public AccountBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
           IBranchBLValidation _branchValidatorBL,
            IRepositoryManager _repositoryManager,
            UserManagerRepository _userManagerRepository,
            IOptions<Jwt> _appSettings,
           RequestHeaderContext _userContext,
            IMailBL _mailBL, IHttpContextAccessor _accessor,
           LinkGenerator _generator, IAccountBLValidation _registerationValidatorBL)
        {
            unitOfWork = _unitOfWork;
            userManagerRepository = _userManagerRepository;
            requestHeaderContext = _userContext;
            jwt = _appSettings.Value;
            repositoryManager = _repositoryManager;
            branchValidatorBL = _branchValidatorBL;
            mailBL = _mailBL;
            accessor = _accessor;
            generator = _generator;
            accountBLValidation = _registerationValidatorBL;
        }      
        public async Task<bool> SignUp(SignUpModel signUpModel)
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

            requestHeaderContext.IsMainBranch = true;

            #region Business Validation

            await accountBLValidation.SignUpValidation(signUpModel);

            #endregion



            signUpModel.UserMobileNumber = MobileHelper.HandleMobile(signUpModel.UserMobileNumber);

            unitOfWork.CreateTransaction();

            #region insert user

            var user = await CreateUser(signUpModel);

            #endregion

            #region Insert Company

            var insertedCompany = repositoryManager.CompanyRepository.Insert(new Company()
            {
                Name = signUpModel.CompanyName,
                IsActive = true,
                CountryId = signUpModel.CompanyCountryId
            });
            await unitOfWork.SaveAsync();
            var companyId = insertedCompany.Id;

            #endregion

            #region Insert Branch

            Branch branch = new()
            {
                CompanyId = companyId,
                Email = signUpModel.CompanyEmail,
                IsActive = true,
                AdminUserId = user.Id,
                Name = signUpModel.CompanyName,
                IsMainBranch = true,
                CountryId = signUpModel.CompanyCountryId,
                Address = signUpModel.CompanyAddress,
            };

            repositoryManager.BranchRepository.Insert(branch);
            await unitOfWork.SaveAsync();

            #region Update User Main Branch

            var getUser = repositoryManager.UserRepository.GetByID(user.Id);
            getUser.MainBranchId = branch.Id;
            unitOfWork.Save();

            #endregion

            #endregion

            #region Insert User Branch

            var userBranch = new UserBranch()
            {

                UserId = user.Id,
                BranchId = branch.Id
            };

            repositoryManager.UserBranchRepository.Insert(userBranch);
            await unitOfWork.SaveAsync();

            #endregion

            #region Send Verification Email

            //Add Token to verify Email 
            var token = await userManagerRepository.GenerateEmailConfirmationTokenAsync(user);
            var emailToken = new { emailtoken = token, email = user.Email };
            var verificationLink = GenerateConfirmEmailLink(emailToken);

            //front end will prepare two mail files (ar and en)
            var verifyEmail = new VerifyEmailModel
            {
                Email = user.Email,
                Subject = "شكرا للتسجيل مع داوم",
                Body = @"<meta charset='UTF-8'>
                                            <title>شكرا للتسجيل مع داوم</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>مرحباً  " + user.FirstName + " " + user.LastName + @"</h1>
                                            <p>شكراً لتسجيلك بالتجربة المجانية مع شركة داوم.</p>
                                            <p>لاستكمال عملية التسجيل، يرجى الضغط على الرابط التالي لتأكيد عنوان البريد الإلكتروني الخاص بك وتفعيل الحساب الجديد:</p>
                                            <p><a href=' " + verificationLink + @" '> اضغط لتأكيد البريد الإلكتروني </a></p>
                                            <p>فريق خدمة العملاء لشركة داوم يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:info@smart-bt.com'>info@smart-bt.com</a></p>
                                            <p> الهاتف: (+20)105210214
                                            </body>
                                            </html>"
            };

            await mailBL.SendEmail(verifyEmail);

            #endregion

            #region Add User Tokens

            var userToken = new UserToken()
            {
                UserId = user.Id,
                LoginProvider = DawemKeys.Email,
                Name = DawemKeys.EmailConfirmationToken,
                Value = token
            };

            repositoryManager.UserTokenRepository.Insert(userToken);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion


        }
        private async Task<MyUser> CreateUser(SignUpModel model)
        {

            string RoleName = DawemKeys.FullAccess;
            var user = new MyUser()
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
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser); //default
            }

            var assignRole = await userManagerRepository.AddToRoleAsync(user, RoleName);
            if (!assignRole.Succeeded)
            {
                throw new BusinessValidationException(DawemKeys.SorryErrorHappenWhileAddingUser);
            }

            return user;
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

            #region Business Validation

            var user = await accountBLValidation.SignInValidation(signInModel);

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

            #region Get Token Model

            TokenModel tokenModelSearchCriteria = new()
            {
                BranchId = signInModel.BranchId,
                UserId = user.Id,
                UserName = user.UserName,
                RememberMe = signInModel.RememberMe,
                Roles = roles,
                ApplicationType = signInModel.ApplicationType
            };

            var tokenData = await GetTokenModel(tokenModelSearchCriteria);

            #endregion

            return tokenData;
        }
        public async Task<TokenDto> GetTokenModel(TokenModel criteria)
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
                Token = token
            };

            #endregion

            return tokenModel;
        }
        private string GenerateConfirmEmailLink(object emailToken)
        {
            var path = generator.GetPathByAction(DawemKeys.VerifyEmail, DawemKeys.Account, emailToken);
            var protocol = accessor.HttpContext.Request.IsHttps ? DawemKeys.Https : DawemKeys.Http;
            var host = accessor.HttpContext.Request.Host.Value;
            var confirmEmailLink = $"{protocol}://{host}{path}";
            return confirmEmailLink;
        }
        public async Task<bool> VerifyEmail(string token, string email)
        {

            var user = await userManagerRepository.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManagerRepository.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                {
                    return false;

                }
            }
            else
            {
                return false;
            }

            return true;
        }
        public async Task<bool> ForgetPassword(ForgetPasswordModel model)
        {
            var user = await userManagerRepository.FindByEmailAsync(model.Email);
            if (user != null || await userManagerRepository.IsEmailConfirmedAsync(user))
            {
                var token = await userManagerRepository.GenerateEmailConfirmationTokenAsync(user);
                var emailToken = new { emailtoken = token, email = user.Email };

                var confirmationLink = GenerateConfirmEmailLink(emailToken);
                VerifyEmailModel verifyEmail = new VerifyEmailModel
                {
                    Subject = requestHeaderContext.Lang == "ar" ? "مرحبًا بك في داوم! أعد تعيين كلمة المرور الخاصة بك" : "Welcome to  Dawem! Reset Your Password",
                    UserName = user.FirstName + " " + user.LastName,
                    Body = string.Format("{0} <a href=\"" + confirmationLink + "\">here</a>",
                    requestHeaderContext.Lang == "ar" ? " يرجى إعادة تعيين كلمة المرور الخاصة بك عن طريق النقر علي" : "Please reset your password by clicking"),
                    Email = user.Email

                };

                await mailBL.SendEmail(verifyEmail);

                return true;
            }

            return false;
        }
        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            var user = await userManagerRepository.FindByIdAsync(model.UserId.ToString()) ?? throw new BusinessValidationException(DawemKeys.NoUserWithSuchName);

            IdentityResult result = await userManagerRepository.ChangePasswordAsync(user, model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                throw new BusinessValidationException(DawemKeys.SorryTheOldPasswordIsNotCorrect);
            }

            return true;
        }

    }
}

