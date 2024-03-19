using Dawem.Helpers;
using Dawem.Models.Dtos.Identities;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentications
{
    public class UserValidator : AbstractValidator<CreatedUser>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterFirstName);
            RuleFor(user => user.LastName).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterLastName);
            RuleFor(user => user.Email).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            RuleFor(user => user.MobileCountryId)
            .GreaterThan(0)
            .WithMessage(LeillaKeys.SorryYouMustChooseMobileCountry);

            RuleFor(user => user.MobileNumber).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterMobileNumber);

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
