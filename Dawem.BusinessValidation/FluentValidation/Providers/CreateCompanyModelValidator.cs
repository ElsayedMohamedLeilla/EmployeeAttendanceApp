using Dawem.Helpers;
using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Departments
{
    public class CreateCompanyModelValidator : AbstractValidator<CreateCompanyModel>
    {
        public CreateCompanyModelValidator()
        {

            RuleFor(model => model.CountryId).
                NotNull().
                WithMessage(LeillaKeys.SorryYouChooseCountry);

            RuleFor(model => model.Name).
                NotNull().
                WithMessage(LeillaKeys.SorryYouMustEnterCompanyName);

            RuleFor(model => model.Email).
                 NotNull().
                WithMessage(LeillaKeys.SorryYouMustEnterCompanyEmail);

            RuleFor(model => model.NumberOfEmployees).
                Must(x => x > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterNumberOfEmployees);

            RuleFor(model => model.Industries).
                 Must(iList => iList.All(i => !string.IsNullOrEmpty(i) && !string.IsNullOrWhiteSpace(i))).
                 When(model => model.Industries != null && model.Industries.Count() > 0).
                 WithMessage(LeillaKeys.SorryYouMustEnterIndustryName);

            RuleFor(model => model.Email).
               Must(EmailHelper.IsValidEmail).
               WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

        }
    }
}
