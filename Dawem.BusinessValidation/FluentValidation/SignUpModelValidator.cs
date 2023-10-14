using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation
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


            RuleFor(signUpModel => signUpModel.Password).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterPassword);
            RuleFor(signUpModel => signUpModel.ConfirmPassword).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(signUpModel => signUpModel).Must(signUpModel => signUpModel.Password == signUpModel.ConfirmPassword).
                  WithMessage(DawemKeys.SorryPasswordAndConfirmPasswordMustEqual);

            RuleFor(signUpModel => signUpModel.UserEmail).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterUserEmail);
            RuleFor(signUpModel => signUpModel.UserMobileNumber).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterUserMobileNumber);
            RuleFor(signUpModel => signUpModel.FirstName).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterFirstName);
            RuleFor(signUpModel => signUpModel.LastName).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterLastName);
        }
    }
}
