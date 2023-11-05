using Dawem.Models.Dtos.Employees.TaskType;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.TaskTypes
{
    public class UpdateTaskTypeModelValidator : AbstractValidator<UpdateTaskTypeModel>
    {
        public UpdateTaskTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterTaskTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterTaskTypeName);

        }
    }
}
