using Dawem.Models.Dtos.Identity;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(signInModel => signInModel.Email).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterEmail);
            RuleFor(signInModel => signInModel.Password).NotNull().
                WithMessage(DawemKeys.SorryYouMustEnterPassword);
            RuleFor(signInModel => (int)signInModel.ApplicationType)
                .Must(applicationType => applicationType > 0 && applicationType < 4).
                WithMessage(DawemKeys.SorryYouMustEnterApplicationType);
        }
    }
}
