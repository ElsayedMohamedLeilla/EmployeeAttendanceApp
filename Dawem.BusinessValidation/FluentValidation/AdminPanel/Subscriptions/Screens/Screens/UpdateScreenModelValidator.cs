using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.Subscriptions.Screens.Screens
{
    public class UpdateScreenModelValidator : AbstractValidator<UpdateScreenModel>
    {
        public UpdateScreenModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterScreenId);

            RuleFor(model => model.NameTranslations).
                Must(nt => nt != null && nt.Count > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterScreenName);

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
