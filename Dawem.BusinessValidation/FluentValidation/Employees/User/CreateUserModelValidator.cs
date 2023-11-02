using Dawem.Helpers;
using Dawem.Models.Dtos.Employees.User;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Employees
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            RuleFor(user => user.Name).NotNull().
                  WithMessage(DawemKeys.SorryYouMustEnterUserName);
            RuleFor(user => user.Email).NotNull().
                 WithMessage(DawemKeys.SorryYouMustEnterEmail);
            RuleFor(user => user.MobileNumber).NotNull().
                 WithMessage(DawemKeys.SorryYouMustEnterMobileNumber);
            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(DawemKeys.SorryYouMustEnterValidEmail);

            RuleFor(user => user.Roles).Must(r=> r != null && r.Count > 0).
               WithMessage(DawemKeys.SorryYouMustEnterOneRoleAtLeast);

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
