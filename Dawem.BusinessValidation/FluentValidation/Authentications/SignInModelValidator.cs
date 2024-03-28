using Dawem.Models.Dtos.Identities;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentications
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

            /*RuleFor(signInModel => signInModel.DeviceToken)
                .Must(d=> !string.IsNullOrEmpty(d) && !string.IsNullOrWhiteSpace(d)).
                WithMessage(LeillaKeys.SorryYouMustEnterDeviceToken);*/
        }
    }
}
