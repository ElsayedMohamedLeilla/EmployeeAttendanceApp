using Dawem.Models.Dtos.Providers;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentications
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
