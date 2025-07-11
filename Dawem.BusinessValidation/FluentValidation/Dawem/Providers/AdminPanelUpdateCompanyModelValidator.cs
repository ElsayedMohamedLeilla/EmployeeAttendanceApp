﻿using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Providers
{
    public class AdminPanelUpdateCompanyModelValidator : AbstractValidator<AdminPanelUpdateCompanyModel>
    {
        public AdminPanelUpdateCompanyModelValidator()
        {
            RuleFor(model => model.Id).
                GreaterThan(0).
                WithMessage(LeillaKeys.SorryYouMustEnterCompanyId);

            RuleFor(model => model.Email).
                 NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterCompanyEmail);

            RuleFor(model => model.Name).
                NotNull().
                WithMessage(LeillaKeys.SorryYouMustEnterCompanyName);

            RuleFor(model => model.Industries).
                 Must(iList => iList.All(i => !string.IsNullOrEmpty(i.Name) && !string.IsNullOrWhiteSpace(i.Name))).
                 When(model => model.Industries != null && model.Industries.Count() > 0).
                 WithMessage(LeillaKeys.SorryYouMustEnterIndustryName);

            RuleFor(model => model.Branches).
                 Must(branches => branches.All(branch => !string.IsNullOrEmpty(branch.Name) && !string.IsNullOrWhiteSpace(branch.Name))).
                 When(model => model.Branches != null && model.Branches.Count() > 0).
                 WithMessage(LeillaKeys.SorryYouMustEnterBranchName);

            RuleFor(model => model.Email).
               Must(EmailHelper.IsValidEmail).
               WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);

            RuleFor(model => model.PreferredLanguageId).
               Must(p => p > 0).
               When(model => model.ImportDefaultData).
               WithMessage(LeillaKeys.SorryYouMustChoosePreferredLanguageWhenChooseImportDefaultData);

            RuleFor(model => model.NumberOfEmployees).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterNumberOfEmployees);
        }
    }
}
