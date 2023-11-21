using Dawem.Models.Dtos.Core.Group;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.Groups
{
    public class UpdateGroupModelValidator : AbstractValidator<UpdateGroupDTO>
    {
        public UpdateGroupModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(AmgadKeys.SorryYouMustEnterGroupId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(AmgadKeys.SorryYouMustEnterGroupName);

        }
    }
}
