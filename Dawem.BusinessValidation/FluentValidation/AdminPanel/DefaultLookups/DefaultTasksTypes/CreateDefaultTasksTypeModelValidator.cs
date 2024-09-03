using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultTasksTypes
{
    public class CreateDefaultTasksTypeModelValidator : AbstractValidator<CreateDefaultTasksTypeDTO>
    {
        public CreateDefaultTasksTypeModelValidator()
        {

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterTasksTypeName);
            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectTaskType);
        }
    }
}
