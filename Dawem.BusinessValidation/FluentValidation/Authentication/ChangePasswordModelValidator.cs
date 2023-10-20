using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(changePasswordModel => changePasswordModel.OldPassword).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterOldPassword);
            RuleFor(changePasswordModel => changePasswordModel.NewPassword).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterNewPassword);
            RuleFor(changePasswordModel => changePasswordModel.UserEmail).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterUserEmail);
            RuleFor(changePasswordModel => changePasswordModel.ConfirmNewPassword).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterConfirmNewPassword);
            RuleFor(changePasswordModel => changePasswordModel)
                .Must(changePasswordModel => changePasswordModel.NewPassword == changePasswordModel.ConfirmNewPassword)
                .WithMessage(DawemKeys.SorryNewPasswordAndConfirmNewPasswordMustEqual);

            RuleFor(changePasswordModel => changePasswordModel)
                .Must(changePasswordModel => changePasswordModel.OldPassword != changePasswordModel.NewPassword)
                .WithMessage(DawemKeys.SorryNewPasswordAndOldPasswordMustNotEqual);
        }
    }
}
