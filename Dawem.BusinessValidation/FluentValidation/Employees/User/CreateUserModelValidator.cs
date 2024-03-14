using Dawem.Helpers;
using Dawem.Models.Dtos.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.User
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            RuleFor(user => user.Name).NotNull().
                  WithMessage(LeillaKeys.SorryYouMustEnterUserName);
            RuleFor(user => user.Email).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            RuleFor(user => user.MobileNumber).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterMobileNumber);

            RuleFor(user => user.MobileCountryId)
                .GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustChooseMobileCountry);

            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

            RuleFor(user => user.Roles).Must(r => r != null && r.Count > 0).
               WithMessage(LeillaKeys.SorryYouMustEnterOneRoleAtLeast);

            RuleFor(user => user.Password).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterPassword);
            RuleFor(user => user.ConfirmPassword).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(user => user.Password).Length(6, 50).
                   WithMessage(LeillaKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword).
                  WithMessage(LeillaKeys.SorryPasswordAndConfirmPasswordMustEqual);

            RuleFor(model => model.ProfileImageFile)
                  .Must(file => file != null && file.Length > 0 && file.ContentType.Contains(LeillaKeys.Image))
                  .When(model => model.ProfileImageFile != null)
                  .WithMessage(LeillaKeys.SorryYouMustUploadImagesOnly);
        }
    }
}
