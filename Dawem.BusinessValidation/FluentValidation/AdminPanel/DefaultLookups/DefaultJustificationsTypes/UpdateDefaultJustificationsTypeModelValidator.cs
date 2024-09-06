using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultJustificationsTypes
{
    public class UpdateDefaultJustificationsTypeModelValidator : AbstractValidator<UpdateDefaultJustificationsTypeDTO>
    {
        public UpdateDefaultJustificationsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterJustificationsTypeId);

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterJustificationsTypeName);

            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectJustificationType);

        }
    }
}
