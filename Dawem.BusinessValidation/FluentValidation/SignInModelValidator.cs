using Dawem.Models.Dtos.Identity;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation
{
    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(signInModel => signInModel.Email).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterEmail);
            RuleFor(signInModel => signInModel.Password).NotNull().
                WithMessage(DawemKeys.SorryYouMustEnterPassword);
            RuleFor(signInModel => signInModel.ApplicationType).NotNull().
                WithMessage(DawemKeys.SorryYouMustEnterPassword);
        }

    }
}
