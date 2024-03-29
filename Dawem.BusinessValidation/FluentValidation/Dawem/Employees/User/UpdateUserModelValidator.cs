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
            RuleFor(user => user.Name).NotNull().
                  WithMessage(LeillaKeys.SorryYouMustEnterUserName);

            RuleFor(user => user.Email).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            RuleFor(user => user.MobileNumber).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterMobileNumber);

            RuleFor(model => model.MobileNumber).
                Must(m => m.IsDigitsOnly()).
                WithMessage(LeillaKeys.SorryYouMustEnterCorrectMobileNumberContainsNumbersOnly);

            RuleFor(user => user.MobileCountryId)
                .GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustChooseMobileCountry);


            RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

            RuleFor(user => user.Roles).Must(r => r != null && r.Count > 0).
               WithMessage(LeillaKeys.SorryYouMustEnterOneRoleAtLeast);

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
