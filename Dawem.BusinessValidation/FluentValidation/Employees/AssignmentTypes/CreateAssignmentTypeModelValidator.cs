using Dawem.Models.Dtos.Employees.AssignmentTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.AssignmentTypes
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
