using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class RequestResetPasswordModelValidator : AbstractValidator<RequestResetPasswordModel>
    {
        public RequestResetPasswordModelValidator()
        {
            RuleFor(changePasswordModel => changePasswordModel.UserEmail).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterUserEmail);
        }
    }
}
