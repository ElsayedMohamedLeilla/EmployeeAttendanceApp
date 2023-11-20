using Dawem.Models.Dtos.Identity;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(signInModel => signInModel.CompanyId).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterCompanyId);

            RuleFor(signInModel => signInModel.Email).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            RuleFor(signInModel => signInModel.Password).NotNull().
                WithMessage(LeillaKeys.SorryYouMustEnterPassword);

            RuleFor(signInModel => signInModel.ApplicationType)
                .IsInEnum().
                WithMessage(LeillaKeys.SorryYouMustEnterApplicationType);
        }
    }
}
