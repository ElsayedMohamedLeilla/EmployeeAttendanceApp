using Dawem.Helpers;
using Dawem.Models.Dtos.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.User
{
    public class UserSignUpModelValidator : AbstractValidator<UserSignUpModel>
    {
        public UserSignUpModelValidator()
        {
            RuleFor(user => user.CompanyId).GreaterThan(0).
                  WithMessage(LeillaKeys.SorryYouMustEnterCompanyId);

            RuleFor(user => user.EmployeeNumber).GreaterThan(0).
                  WithMessage(LeillaKeys.SorryYouMustEnterEmployeeNumber);

            RuleFor(user => user.Name).NotNull().
                  WithMessage(LeillaKeys.SorryYouMustEnterUserName);

            RuleFor(user => user.Email).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            RuleFor(user => user.MobileNumber).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterMobileNumber);

            RuleFor(user => user.MobileCountryId)
                .GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustChooseMobileCountry);
            
            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

            RuleFor(user => user.Password).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterPassword);

            RuleFor(user => user.ConfirmPassword).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(user => user.Password).Length(6, 50).
                   WithMessage(LeillaKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword).
                  WithMessage(LeillaKeys.SorryPasswordAndConfirmPasswordMustEqual);
        }
    }
}
