using Dawem.Models.Dtos.Dawem.Employees.TaskTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.TaskTypes
{
    public class CreateTaskTypeModelValidator : AbstractValidator<CreateTaskTypeModel>
    {
        public CreateTaskTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterTaskTypeName);

        }
    }
}
