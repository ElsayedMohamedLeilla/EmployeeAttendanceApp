using Dawem.Models.Dtos.Core.Groups;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.Zones
{
    public class UpdateZoneModelValidator : AbstractValidator<UpdateGroupDTO>
    {
        public UpdateZoneModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(AmgadKeys.SorryYouMustEnterGroupId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(AmgadKeys.SorryYouMustEnterGroupName);

        }
    }
}
