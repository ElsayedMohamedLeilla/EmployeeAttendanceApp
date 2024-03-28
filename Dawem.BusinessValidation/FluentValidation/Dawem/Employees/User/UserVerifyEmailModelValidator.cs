using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.User
{
    public class UserVerifyEmailModelValidator : AbstractValidator<UserVerifyEmailModel>
    {
        public UserVerifyEmailModelValidator()
        {
            RuleFor(user => user.UserId).GreaterThan(0).
                  WithMessage(LeillaKeys.SorryYouMustEnterUserId);
            RuleFor(user => user.VerificationCode).Must(v => !string.IsNullOrEmpty(v) && !string.IsNullOrWhiteSpace(v)).
                  WithMessage(LeillaKeys.SorryYouMustEnterVerificationCode);
        }
    }
}
