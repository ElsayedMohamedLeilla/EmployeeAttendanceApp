using Dawem.Models.Dtos.Dawem.Employees.AssignmentTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.AssignmentTypes
{
    public class UpdateAssignmentTypeModelValidator : AbstractValidator<UpdateAssignmentTypeModel>
    {
        public UpdateAssignmentTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterAssignmentTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterAssignmentTypeName);

        }
    }
}
