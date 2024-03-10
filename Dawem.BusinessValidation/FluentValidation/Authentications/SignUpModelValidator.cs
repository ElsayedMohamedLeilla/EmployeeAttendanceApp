using Dawem.Helpers;
using Dawem.Models.Dtos.Providers;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentications
{
    public class SignUpModelValidator : AbstractValidator<SignUpModel>
    {
        public SignUpModelValidator()
        {
            RuleFor(signUpModel => signUpModel.CompanyName).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterCompanyName);
            RuleFor(signUpModel => signUpModel.CompanyCountryId).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustChooseCountry);
            RuleFor(signUpModel => signUpModel.CompanyAddress).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterCompanyAddress);

            RuleFor(signUpModel => signUpModel.CompanyEmail).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterCompanyEmail);

            RuleFor(signUpModel => signUpModel.CompanyEmail).Must(EmailHelper.IsValidEmail).
                 WithMessage(LeillaKeys.SorryYouMustEnterValidCompanyEmail);


            RuleFor(signUpModel => signUpModel.Password).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterPassword);
            RuleFor(signUpModel => signUpModel.ConfirmPassword).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(signUpModel => signUpModel.Password).Length(6, 50).
                  WithMessage(LeillaKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(signUpModel => signUpModel).Must(signUpModel => signUpModel.Password == signUpModel.ConfirmPassword).
                  WithMessage(LeillaKeys.SorryPasswordAndConfirmPasswordMustEqual);



            RuleFor(signUpModel => signUpModel.UserEmail).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterUserEmail);

            RuleFor(signUpModel => signUpModel.UserEmail).Must(EmailHelper.IsValidEmail).
                   WithMessage(LeillaKeys.SorryYouMustEnterValidUserEmail);

            RuleFor(signUpModel => signUpModel.UserMobileNumber).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterUserMobileNumber);
            RuleFor(signUpModel => signUpModel.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterUserName);


            RuleFor(signUpModel => signUpModel.NumberOfEmployees)
                .Must(n => n > 0)
                .WithMessage(LeillaKeys.SorryYouMustEnterNumberOfEmployees);

            RuleFor(signUpModel => signUpModel.SubscriptionDurationInMonths)
                .Must(s => s > 0)
                .WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionDurationInMonths);
        }
    }
}
