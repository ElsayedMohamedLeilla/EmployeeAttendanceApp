using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Providers
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
                 Must(iList => iList.All(i => !string.IsNullOrEmpty(i.Name) && !string.IsNullOrWhiteSpace(i.Name))).
                 When(model => model.Industries != null && model.Industries.Count() > 0).
                 WithMessage(LeillaKeys.SorryYouMustEnterIndustryName);

            RuleFor(model => model.Email).
               Must(EmailHelper.IsValidEmail).
               WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

            RuleFor(model => model.PreferredLanguageId).
               Must(p => p > 0).
               When(model => model.ImportDefaultData).
               WithMessage(LeillaKeys.SorryYouMustChoosePreferredLanguageWhenChooseImportDefaultData);

            RuleFor(model => model.Branches).
                 Must(branches => branches.All(branch => !string.IsNullOrEmpty(branch.Name) && !string.IsNullOrWhiteSpace(branch.Name))).
                 When(model => model.Branches != null && model.Branches.Count() > 0).
                 WithMessage(LeillaKeys.SorryYouMustEnterBranchName);

        }
    }
}
