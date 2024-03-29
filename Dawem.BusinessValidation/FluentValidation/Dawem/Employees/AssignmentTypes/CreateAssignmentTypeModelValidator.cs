using Dawem.Models.Dtos.Dawem.Employees.AssignmentTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.AssignmentTypes
{
    public class CreateAssignmentTypeModelValidator : AbstractValidator<CreateAssignmentTypeModel>
    {
        public CreateAssignmentTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterAssignmentTypeName);

        }
    }
}
