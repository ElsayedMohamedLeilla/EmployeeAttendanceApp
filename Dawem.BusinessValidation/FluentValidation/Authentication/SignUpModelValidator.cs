using Dawem.Helpers;
using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class SignUpModelValidator : AbstractValidator<SignUpModel>
    {
        public SignUpModelValidator()
        {
            RuleFor(signUpModel => signUpModel.CompanyName).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterCompanyName);
            RuleFor(signUpModel => signUpModel.CompanyCountryId).GreaterThan(0).
                   WithMessage(DawemKeys.SorryYouMustChooseCountry);
            RuleFor(signUpModel => signUpModel.CompanyAddress).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterCompanyAddress);

            RuleFor(signUpModel => signUpModel.CompanyEmail).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterCompanyEmail);

            RuleFor(signUpModel => signUpModel.CompanyEmail).Must(EmailHelper.IsValidEmail).
                 WithMessage(DawemKeys.SorryYouMustEnterValidCompanyEmail);


            RuleFor(signUpModel => signUpModel.Password).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterPassword);
            RuleFor(signUpModel => signUpModel.ConfirmPassword).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(signUpModel => signUpModel.Password).Length(6, 50).
                  WithMessage(DawemKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(signUpModel => signUpModel).Must(signUpModel => signUpModel.Password == signUpModel.ConfirmPassword).
                  WithMessage(DawemKeys.SorryPasswordAndConfirmPasswordMustEqual);



            RuleFor(signUpModel => signUpModel.UserEmail).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterUserEmail);

            RuleFor(signUpModel => signUpModel.UserEmail).Must(EmailHelper.IsValidEmail).
                   WithMessage(DawemKeys.SorryYouMustEnterValidUserEmail);

            RuleFor(signUpModel => signUpModel.UserMobileNumber).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterUserMobileNumber);
            RuleFor(signUpModel => signUpModel.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterUserName);
        }
    }
}
