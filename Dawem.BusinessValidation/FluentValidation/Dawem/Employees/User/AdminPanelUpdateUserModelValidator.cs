using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.User
{
    public class AdminPanelUpdateUserModelValidator : AbstractValidator<AdminPanelUpdateUserModel>
    {
        public AdminPanelUpdateUserModelValidator()
        {
            RuleFor(user => user.Id).NotNull().
                  WithMessage(LeillaKeys.SorryYouMustEnterUserId);

            RuleFor(user => user.Name).NotNull().
                  WithMessage(LeillaKeys.SorryYouMustEnterUserName);

            RuleFor(user => user.Email).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

            RuleFor(user => user.Responsibilities).Must(r => r != null && r.Count > 0).
               WithMessage(LeillaKeys.SorryYouMustEnterOneResponsibilityAtLeast);

            RuleFor(model => model.ProfileImageFile)
                  .Must(file => file != null && file.Length > 0 && file.ContentType.Contains(LeillaKeys.Image))
                  .When(model => model.ProfileImageFile != null)
                  .WithMessage(LeillaKeys.SorryYouMustUploadImagesOnly);

            /*RuleFor(user => user.Password).Length(6, 50)
                 .When(user => user.Password != null)
                 .WithMessage(DawemKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword)
                .When(user => user.Password != null || user.ConfirmPassword != null)
                .WithMessage(DawemKeys.SorryPasswordAndConfirmPasswordMustEqual);*/
        }
    }
}
