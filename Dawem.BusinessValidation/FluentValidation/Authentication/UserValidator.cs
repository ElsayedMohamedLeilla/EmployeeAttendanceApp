using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Dtos.Identity;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class UserValidator : AbstractValidator<CreatedUser>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterFirstName);
            RuleFor(user => user.LastName).NotNull().
                 WithMessage(DawemKeys.SorryYouMustEnterLastName);
            RuleFor(user => user.Email).NotNull().
                 WithMessage(DawemKeys.SorryYouMustEnterEmail);
            RuleFor(user => user.MobileNumber).NotNull().
                 WithMessage(DawemKeys.SorryYouMustEnterMobileNumber);
            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(DawemKeys.SorryYouMustEnterValidEmail);

            RuleFor(user => user.Password).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterPassword);
            RuleFor(user => user.ConfirmPassword).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(user => user.Password).Length(6, 50).
                   WithMessage(DawemKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword).
                  WithMessage(DawemKeys.SorryPasswordAndConfirmPasswordMustEqual);

        }
    }
}
