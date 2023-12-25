using Dawem.Models.Dtos.Providers;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentications
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(model => model.ResetToken).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterResetToken);
            RuleFor(model => model.UserEmail).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterUserEmail);
            RuleFor(model => model.NewPassword).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterNewPassword);
            RuleFor(model => model.ConfirmNewPassword).NotNull().
                    WithMessage(LeillaKeys.SorryYouMustEnterConfirmNewPassword);
            RuleFor(user => user.NewPassword).Length(6, 50)
                 .When(user => user.NewPassword != null)
                 .WithMessage(LeillaKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);
            RuleFor(model => model)
                .Must(model => model.NewPassword == model.ConfirmNewPassword)
                .WithMessage(LeillaKeys.SorryNewPasswordAndConfirmNewPasswordMustEqual);
        }
    }
}
