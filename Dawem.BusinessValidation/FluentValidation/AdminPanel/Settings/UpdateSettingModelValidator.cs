using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.Subscriptions.Plans
{
    public class UpdateSettingModelValidator : AbstractValidator<UpdateSettingModel>
    {
        public UpdateSettingModelValidator()
        {
            RuleFor(model => model.Settings).
                Must(s => s != null && s.Count > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterSettings);

            RuleForEach(x => x.Settings).
                SetValidator(new UpdateSettingDTOValidator());
        }
        public class UpdateSettingDTOValidator : AbstractValidator<UpdateSettingDTO>
        {
            public UpdateSettingDTOValidator()
            {
                RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterSettingId);

                RuleFor(model => model.SettingType).
                    GreaterThan(-1).
                    WithMessage(LeillaKeys.SorryYouMustEnterSettingType);
            }
        }
    }
}
