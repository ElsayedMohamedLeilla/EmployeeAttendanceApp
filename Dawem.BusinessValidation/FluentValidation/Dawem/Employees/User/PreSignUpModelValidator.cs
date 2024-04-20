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
        }
    }
}
