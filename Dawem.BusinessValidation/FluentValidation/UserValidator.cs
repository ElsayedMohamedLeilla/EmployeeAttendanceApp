using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterFirstName);
            RuleFor(user => user.LastName).Must(EmailHelper.IsValidEmail).
                 WithMessage(DawemKeys.SorryYouMustEnterLastName);
            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                 WithMessage(DawemKeys.SorryYouMustEnterEmail);
            RuleFor(user => user.MobileNumber).Must(EmailHelper.IsValidEmail).
                 WithMessage(DawemKeys.SorryYouMustEnterMobileNumber);
            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(DawemKeys.SorryYouMustEnterValidEmail);

        }
    }
}
