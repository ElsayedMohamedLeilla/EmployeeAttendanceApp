using Dawem.Models.Dtos.Providers;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentications
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(changePasswordModel => changePasswordModel.OldPassword).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterOldPassword);
            RuleFor(changePasswordModel => changePasswordModel.NewPassword).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterNewPassword);
            RuleFor(changePasswordModel => changePasswordModel.UserEmail).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterUserEmail);
            RuleFor(changePasswordModel => changePasswordModel.ConfirmNewPassword).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterConfirmNewPassword);
            RuleFor(changePasswordModel => changePasswordModel)
                .Must(changePasswordModel => changePasswordModel.NewPassword == changePasswordModel.ConfirmNewPassword)
                .WithMessage(LeillaKeys.SorryNewPasswordAndConfirmNewPasswordMustEqual);

            RuleFor(changePasswordModel => changePasswordModel)
                .Must(changePasswordModel => changePasswordModel.OldPassword != changePasswordModel.NewPassword)
                .WithMessage(LeillaKeys.SorryNewPasswordAndOldPasswordMustNotEqual);
        }
    }
}
