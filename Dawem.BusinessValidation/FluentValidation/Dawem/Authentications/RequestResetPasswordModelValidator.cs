using Dawem.Models.Dtos.Dawem.Providers;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Authentications
{
    public class RequestResetPasswordModelValidator : AbstractValidator<RequestResetPasswordModel>
    {
        public RequestResetPasswordModelValidator()
        {
            RuleFor(changePasswordModel => changePasswordModel.UserEmail).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterUserEmail);
        }
    }
}
