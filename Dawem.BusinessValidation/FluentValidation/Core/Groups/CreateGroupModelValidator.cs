using Dawem.Models.Dtos.Core.Group;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.Groups
{
    public class CreateGroupModelValidator : AbstractValidator<CreateGroupDTO>
    {
        public CreateGroupModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(AmgadKeys.SorryYouMustEnterGroupName);

        }
    }
}
