using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultTasksTypes
{
    public class UpdateDefaultTasksTypeModelValidator : AbstractValidator<UpdateDefaultTasksTypeDTO>
    {
        public UpdateDefaultTasksTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(AmgadKeys.SorryYouMustEnterTasksTypeId);

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterTasksTypeName);

            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectTaskType);

        }
    }
}
