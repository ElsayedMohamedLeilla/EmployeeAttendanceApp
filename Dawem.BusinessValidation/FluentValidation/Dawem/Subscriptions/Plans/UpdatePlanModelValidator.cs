using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Subscriptions.Plans
{
    public class UpdatePlanModelValidator : AbstractValidator<UpdatePlanModel>
    {
        public UpdatePlanModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterPlanId);

            RuleFor(model => model.MinNumberOfEmployees).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanMinNumberOfEmployees);

            RuleFor(model => model.MaxNumberOfEmployees).
                Must(x => x > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanMaxNumberOfEmployees);

            RuleFor(model => model.EmployeeCost).
                Must(x => x > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanEmployeeCost);

            RuleFor(model => model).
                Must(x => x.MinNumberOfEmployees <= x.MaxNumberOfEmployees).
                WithMessage(LeillaKeys.SorryPlanMinNumberOfEmployeesMustLessThanOrEqualMaxNumberOfEmployees);

            RuleFor(model => model.NameTranslations).
                Must(nt => nt != null && nt.Count > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterPlanName);

            RuleFor(model => model.NameTranslations).
                Must(nt => nt.All(n => n.LanguageId > 0)).
                WithMessage(LeillaKeys.SorryYouMustChooseLanguageWithName);

            RuleFor(model => model.NameTranslations).
                Must(nt => nt.All(n => !string.IsNullOrEmpty(n.Name) && !string.IsNullOrWhiteSpace(n.Name))).
                WithMessage(LeillaKeys.SorryYouMustEnterName);

            RuleFor(model => model.NameTranslations).
                Must(nt => nt.GroupBy(nt => nt.LanguageId).ToList().All(g => g.Count() == 1)).
                WithMessage(LeillaKeys.SorryYouMustNotRepeatLanguagesWithNames);
        }
    }
}
