using Dawem.Models.Dtos.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.User
{
    public class SendVerificationCodeModelValidator : AbstractValidator<SendVerificationCodeModel>
    {
        public SendVerificationCodeModelValidator()
        {
            RuleFor(user => user.UserId).GreaterThan(0).
                  WithMessage(LeillaKeys.SorryYouMustEnterUserId);
        }
    }
}
