﻿using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.User
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            //RuleFor(user => user.Name).NotNull().
            //      WithMessage(LeillaKeys.SorryYouMustEnterUserName);
            //RuleFor(user => user.Email).NotNull().
            //     WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            //RuleFor(user => user.MobileNumber).NotNull().
            //     WithMessage(LeillaKeys.SorryYouMustEnterMobileNumber);

            //RuleFor(model => model.MobileNumber).
            //    Must(m => m.IsDigitsOnly()).
            //    WithMessage(LeillaKeys.SorryYouMustEnterCorrectMobileNumberContainsNumbersOnly);

            //RuleFor(user => user.MobileCountryId)
            //    .GreaterThan(0)
            //    .WithMessage(LeillaKeys.SorryYouMustChooseMobileCountry);

            //RuleFor(user => user.Email).Must(EmailHelper.IsValidEmail).
            //    WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

            RuleFor(user => user.Responsibilities).
                Must(r => r != null && r.Count > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterOneResponsibilityAtLeast);

            RuleFor(user => user.EmployeeId).
               Must(r => r > 0).
               WithMessage(AmgadKeys.SorryYouMustChooseEmployeeToThisUser);

            RuleFor(user => user.Password).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterPassword);
            RuleFor(user => user.ConfirmPassword).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(user => user.Password).Length(6, 50).
                   WithMessage(LeillaKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(user => user).Must(user => user.Password == user.ConfirmPassword).
                  WithMessage(LeillaKeys.SorryPasswordAndConfirmPasswordMustEqual);

            //RuleFor(model => model.ProfileImageFile)
            //      .Must(file => file != null && file.Length > 0 && file.ContentType.Contains(LeillaKeys.Image))
            //      .When(model => model.ProfileImageFile != null)
            //      .WithMessage(LeillaKeys.SorryYouMustUploadImagesOnly);
        }
    }
}
