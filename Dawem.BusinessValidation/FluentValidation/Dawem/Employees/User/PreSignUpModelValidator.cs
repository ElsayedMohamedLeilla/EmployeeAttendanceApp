using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.User
{
    public class PreSignUpModelValidator : AbstractValidator<PreSignUpDTO>
    {
        public PreSignUpModelValidator()
        {
            RuleFor(user => user.EmployeeNumber).GreaterThan(0).
                  WithMessage(LeillaKeys.SorryYouMustEnterEmployeeNumber);

            RuleFor(user => user.CompanyVerificationCode).NotNull().
                  WithMessage(AmgadKeys.SorryYouMustEnterCompanyVerificationCode);

            RuleFor(signUpModel => signUpModel.Password).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterPassword);

            RuleFor(signUpModel => signUpModel.ConfirmPassword).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterConfirmPassword);

            RuleFor(signUpModel => signUpModel.Password).Length(6, 50).
                  WithMessage(LeillaKeys.SorryYouMustEnterPasswordWithMinimumLengthOf6Charachters);

            RuleFor(signUpModel => signUpModel).Must(signUpModel => signUpModel.Password == signUpModel.ConfirmPassword).
                  WithMessage(LeillaKeys.SorryPasswordAndConfirmPasswordMustEqual);
        }
    }
}
