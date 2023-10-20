using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(model => model.ResetToken).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterResetToken);
            RuleFor(model => model.UserEmail).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterUserEmail);
            RuleFor(model => model.NewPassword).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterNewPassword);
            RuleFor(model => model.ConfirmNewPassword).NotNull().
                    WithMessage(DawemKeys.SorryYouMustEnterConfirmNewPassword);
            RuleFor(model => model)
                .Must(model => model.NewPassword == model.ConfirmNewPassword)
                .WithMessage(DawemKeys.SorryNewPasswordAndConfirmNewPasswordMustEqual);
        }
    }
}
