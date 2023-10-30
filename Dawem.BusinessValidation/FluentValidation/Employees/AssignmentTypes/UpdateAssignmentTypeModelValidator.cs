using Dawem.Models.Dtos.Employees.AssignmentType;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.AssignmentTypes
{
    public class UpdateAssignmentTypeModelValidator : AbstractValidator<UpdateAssignmentTypeModel>
    {
        public UpdateAssignmentTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(DawemKeys.SorryYouMustEnterAssignmentTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterAssignmentTypeName);

        }
    }
}
