using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Provider;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.DtosMappers;
using Dawem.Models.Exceptions;
using Dawem.Models.Generic;
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
    public class AuthenticationBL : IAuthenticationBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly UserManagerRepository userManagerRepository;
        private readonly Jwt jwt;
        private readonly RequestInfo requestHeaderContext;
        private readonly IMailBL mailBL;
        private readonly IHttpContextAccessor accessor;
        private readonly LinkGenerator generator;
        private readonly IAccountBLValidation accountBLValidation;
        private readonly IBranchBLValidation branchValidatorBL;
        private readonly IRepositoryManager repositoryManager;
        public AuthenticationBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
           IBranchBLValidation _branchValidatorBL,
            IRepositoryManager _repositoryManager,
            UserManagerRepository _userManagerRepository,
            IOptions<Jwt> _appSettings,
           RequestInfo _userContext,
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
            requestHeaderContext.IsMainBranch = true;

            #region Business Validation

            await accountBLValidation.SignUpValidation(signUpModel);

            #endregion

            signUpModel.UserMobileNumber = MobileHelper.HandleMobile(signUpModel.UserMobileNumber);

            unitOfWork.CreateTransaction();

            #region Insert User

            var user = await CreateUser(signUpModel);

            #endregion

            #region Insert Company

            #region Set Company Code

            var getNextCode = await repositoryManager.CompanyRepository
                .Get(e => !e.IsDeleted)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            #region Set Company Identity Code

            var identityCode = StringHelper.RandomString(10);
            var checkForRandomString = await repositoryManager.CompanyRepository
                .Get(e => !e.IsDeleted && e.IdentityCode == identityCode)
                .AnyAsync();

            while (checkForRandomString)
            {
                identityCode = StringHelper.RandomString(10);
                checkForRandomString = await repositoryManager.CompanyRepository
                .Get(e => !e.IsDeleted && e.IdentityCode == identityCode)
                .AnyAsync();
            }

            #endregion

            var insertedCompany = repositoryManager.CompanyRepository.Insert(new Company()
            {
                IdentityCode = identityCode,
                Code = getNextCode,
                Name = signUpModel.CompanyName,
                IsActive = true,
                AddUserId = user.Id,
                CountryId = signUpModel.CompanyCountryId,
                Email = signUpModel.CompanyEmail
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

            #endregion

            #region Insert Department And Employee

            var employee = repositoryManager.EmployeeRepository.Insert(new Employee
            {
                CompanyId = companyId,
                Department = new Department
                {
                    CompanyId = companyId,
                    Code = 1,
                    IsActive = true,
                    Name = TranslationHelper.GetTranslation(LeillaKeys.MainDepartment, LeillaKeys.Ar)
                },
                Code = 1,
                JoiningDate = DateTime.UtcNow,
                IsActive = true,
                Name = TranslationHelper.GetTranslation(LeillaKeys.AdminEmployee, LeillaKeys.Ar)
            });
            await unitOfWork.SaveAsync();

            #endregion

            #region Update User

            var getUser = repositoryManager.UserRepository.GetByID(user.Id);
            getUser.BranchId = branch.Id;
            getUser.CompanyId = insertedCompany.Id;
            getUser.EmployeeId = employee.Id;

            await unitOfWork.SaveAsync();

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
                                            <h1>مرحباً  " + user.Name + @"</h1>
                                            <p>شكراً لتسجيلك بالتجربة المجانية مع شركة داوم.</p>
                                            <p>لاستكمال عملية التسجيل، يرجى الضغط على الرابط التالي لتأكيد عنوان البريد الإلكتروني الخاص بك وتفعيل الحساب الجديد:</p>
                                            <p><a href=' " + verificationLink + @" '> اضغط لتأكيد البريد الإلكتروني </a></p>
                                            <p>فريق خدمة العملاء لشركة داوم يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:dawem.app.developers@gmail.com'>dawem.app.developers@gmail.com</a></p>
                                            <p> الهاتف: (+20)01234567
                                            </body>
                                            </html>"
            };

            await mailBL.SendEmail(verifyEmail);

            #endregion

            #region Add User Tokens

            var userToken = new UserToken()
            {
                UserId = user.Id,
                LoginProvider = LeillaKeys.Email,
                Name = LeillaKeys.EmailConfirmationToken,
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

            string RoleName = LeillaKeys.Admin;
            var user = new MyUser()
            {
                UserName = model.UserEmail,
                Email = model.UserEmail,
                Name = model.Name,
                Code = 1,
                MobileNumber = model.UserMobileNumber,
                IsAdmin = true,
                IsActive = true
            };

            var createUserResponse = await userManagerRepository.CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                //var errors1= string.Join(DawemKeys.Comma, createUserResponse.Errors.Select(x => x.Description).FirstOrDefault());
                //var errors2 = createUserResponse.Errors.Select(x => x.Code).FirstOrDefault();
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser); //default
            }

            var assignRole = await userManagerRepository.AddToRoleAsync(user, RoleName);
            if (!assignRole.Succeeded)
            {
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileAddingUser);
            }

            return user;
        }
        public async Task<TokenDto> SignIn(SignInModel signInModel)
        {
            #region Business Validation

            var user = await accountBLValidation.SignInValidation(signInModel);

            #endregion

            #region Handle User Role

            var roles = await userManagerRepository.GetRolesAsync(user);
            if (roles.FirstOrDefault(r => r == LeillaKeys.FullAccess) == null)
            {
                var addingToRoleResult = await userManagerRepository.AddToRoleAsync(user, LeillaKeys.FullAccess);

                if (addingToRoleResult.Succeeded)
                {
                    roles.Add(LeillaKeys.FullAccess);
                }
            }

            #endregion

            #region Get Token Model

            TokenModel tokenModelSearchCriteria = new()
            {
                UserId = user.Id,
                UserName = user.UserName,
                RememberMe = signInModel.RememberMe,
                Roles = roles,
                CompanyId = user.CompanyId,
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
                new Claim(LeillaKeys.UserId, criteria.UserId.ToString()),
                new Claim(LeillaKeys.CompanyId , criteria.CompanyId.ToString()),
                new Claim(LeillaKeys.ApplicationType , criteria.ApplicationType.ToString())
            });
            if (criteria.RememberMe)
            {
                claimsIdentity.AddClaim(new Claim(LeillaKeys.RememberMe, LeillaKeys.True));
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
                Expires = criteria.RememberMe ? DateTime.UtcNow.AddDays(3) : DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwt.Issuer,
                Audience = jwt.Issuer
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            var tokenData = await FormateToken(criteria.UserId, criteria.BranchId, token);

            #endregion

            return tokenData;
        }
        public async Task<TokenDto> FormateToken(int? userId, int branchId, string token)
        {
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
            var path = generator.GetPathByAction(LeillaKeys.VerifyEmail, LeillaKeys.Account, emailToken);
            var protocol = accessor.HttpContext.Request.IsHttps ? LeillaKeys.Https : LeillaKeys.Http;
            var host = accessor.HttpContext.Request.Host.Value;
            var confirmEmailLink = $"{protocol}://{host}{path}";
            return confirmEmailLink;
        }
        private static string GetResetPasswordLink(ResetPasswordToken emailToken)
        {
            var path = "resetpassword?resetToken=" + emailToken.Token + "&email=" + emailToken.Email;
            var protocol = LeillaKeys.Https;
            var host = "pro.dawem.app/";
            var resetPasswordLink = $"{protocol}://{host}{path}";
            return resetPasswordLink;
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
        public async Task<bool> RequestResetPassword(RequestResetPasswordModel model)
        {
            var user = await userManagerRepository.FindByNameAsync(model.UserEmail) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindUserWithEnteredEmail);

            if (!user.EmailConfirmed)
            {
                throw new BusinessValidationException(LeillaKeys.SorryUserEmailNotConfirmed);
            }

            var passwordResetToken = await userManagerRepository.GeneratePasswordResetTokenAsync(user);
            var tokenObject = new ResetPasswordToken { Token = passwordResetToken, Email = user.Email };

            var confirmationLink = GetResetPasswordLink(tokenObject);
            var verifyEmail = new VerifyEmailModel
            {
                Subject = requestHeaderContext.Lang == "ar" ? "مرحبًا بك في داوم! أعد تعيين كلمة المرور الخاصة بك" : "Welcome to  Dawem! Reset Your Password",
                UserName = user.Name,
                Body = string.Format("{0} <a href=\"" + confirmationLink + "\"> " + (requestHeaderContext.Lang == "ar" ? "تعيين كلمة المرور" : "Reset Password") + "</a>",
                requestHeaderContext.Lang == "ar" ? " يرجى إعادة تعيين كلمة المرور الخاصة بك عن طريق النقر علي" : "Please reset your password by clicking"),
                Email = user.Email
            };

            await mailBL.SendEmail(verifyEmail);

            return true;
        }
        public async Task<bool> ResetPassword(ResetPasswordModel model)
        {
            var user = await userManagerRepository.FindByNameAsync(model.UserEmail) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindUserWithEnteredEmail);

            var resetPasswordResult = await userManagerRepository.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);

            if (!resetPasswordResult.Succeeded)
            {
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileResetPassword);
            }
            return false;
        }
        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            var user = await userManagerRepository.FindByNameAsync(model.UserEmail) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindUserWithEnteredEmail);

            bool checkPasswordAsyncRes = await userManagerRepository.CheckPasswordAsync(user, model.OldPassword);
            if (!checkPasswordAsyncRes)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPasswordIncorrectEnterCorrectPasswordForSelectedUser);
            }

            IdentityResult result = await userManagerRepository.ChangePasswordAsync(user, model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhenChangePassword);
            }

            return true;
        }
        public async Task<int> VerifyCompanyCode(string identityCode)
        {
            if (string.IsNullOrEmpty(identityCode) || string.IsNullOrWhiteSpace(identityCode))
            {
                throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterCompanyCode);
            }

            var companyId = await repositoryManager.CompanyRepository.Get(c => !c.IsDeleted && c.IdentityCode == identityCode)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (companyId <= 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoCompanyWithEnteredCode);
            }

            return companyId;

        }
    }
}

