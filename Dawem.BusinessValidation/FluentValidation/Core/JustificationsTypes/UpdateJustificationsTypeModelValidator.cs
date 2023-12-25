using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.JustificationsTypes
{
    public class UpdateJustificationsTypeModelValidator : AbstractValidator<UpdateJustificationsTypeDTO>
    {
        public UpdateJustificationsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterJustificationsTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterJustificationsTypeName);

        }
    }
}
