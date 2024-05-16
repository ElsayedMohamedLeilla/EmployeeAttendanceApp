using Dawem.Models.Dtos.Dawem.Identities;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Authentications
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

            RuleFor(signInModel => signInModel.FCMToken)
                .Must(d=> !string.IsNullOrEmpty(d) && !string.IsNullOrWhiteSpace(d) && d.Length > 100).
                WithMessage(LeillaKeys.SorryYouMustEnterDeviceNotificationsToken);
        }
    }
}
