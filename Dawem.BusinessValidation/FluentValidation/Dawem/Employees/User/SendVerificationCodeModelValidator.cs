using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.User
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
