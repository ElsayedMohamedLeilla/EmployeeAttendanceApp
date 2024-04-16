using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.User
{
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
        {
            RuleFor(user => user.Id).NotNull().
                  WithMessage(LeillaKeys.SorryYouMustEnterUserId);

            RuleFor(user => user.EmployeeId).NotNull().
                 WithMessage(AmgadKeys.SorryYouMustChooseEmployeeToThisUser);

            RuleFor(user => user.Responsibilities).Must(r => r != null && r.Count > 0).
               WithMessage(LeillaKeys.SorryYouMustEnterOneResponsibilityAtLeast);


            /*RuleFor(user => user.Password).Length(6, 50)
                 .When(user => user.Password != null)
                 .WithMessage(DawemKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword)
                .When(user => user.Password != null || user.ConfirmPassword != null)
                .WithMessage(DawemKeys.SorryPasswordAndConfirmPasswordMustEqual);*/
        }
    }
}
