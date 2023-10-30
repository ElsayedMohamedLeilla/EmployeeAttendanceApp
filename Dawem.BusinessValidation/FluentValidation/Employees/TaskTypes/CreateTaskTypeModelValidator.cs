using Dawem.Models.Dtos.Employees.AssignmentType;
using Dawem.Models.Dtos.Employees.TaskType;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.TaskTypes
{
    public class CreateTaskTypeModelValidator : AbstractValidator<CreateTaskTypeModel>
    {
        public CreateTaskTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterTaskTypeName);

        }
    }
}
