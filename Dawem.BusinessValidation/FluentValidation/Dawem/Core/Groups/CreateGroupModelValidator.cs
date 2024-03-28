using Dawem.Models.Dtos.Dawem.Core.Groups;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.Groups
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
