using Dawem.Helpers;
using Dawem.Models.Dtos.Employees.User;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Employees
{
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
        {
            RuleFor(user => user.Name).NotNull().
                  WithMessage(DawemKeys.SorryYouMustEnterUserName);
            RuleFor(user => user.Email).NotNull().
                 WithMessage(DawemKeys.SorryYouMustEnterEmail);
            RuleFor(user => user.MobileNumber).NotNull().
                 WithMessage(DawemKeys.SorryYouMustEnterMobileNumber);
            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(DawemKeys.SorryYouMustEnterValidEmail);

            /*RuleFor(user => user.Password).Length(6, 50)
                 .When(user => user.Password != null)
                 .WithMessage(DawemKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword)
                .When(user => user.Password != null || user.ConfirmPassword != null)
                .WithMessage(DawemKeys.SorryPasswordAndConfirmPasswordMustEqual);*/
        }
    }
}
