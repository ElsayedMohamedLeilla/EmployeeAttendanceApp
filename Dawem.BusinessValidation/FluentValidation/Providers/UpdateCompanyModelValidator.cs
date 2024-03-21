using Dawem.Helpers;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Departments
{
    public class UpdateCompanyModelValidator : AbstractValidator<UpdateCompanyModel>
    {
        public UpdateCompanyModelValidator()
        {
            RuleFor(model => model.Id).
                GreaterThan(0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanId);

            RuleFor(model => model.Email).
                 NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterCompanyEmail);

            RuleFor(model => model.Industries).
                 Must(iList=> iList.All(i=> !string.IsNullOrEmpty(i) && !string.IsNullOrWhiteSpace(i))).
                 When(model => model.Industries != null && model.Industries.Count() > 0).
                 WithMessage(LeillaKeys.SorryYouMustEnterIndustryName);

            RuleFor(model => model.Email).
               Must(EmailHelper.IsValidEmail).
               WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);
        }
    }
}
