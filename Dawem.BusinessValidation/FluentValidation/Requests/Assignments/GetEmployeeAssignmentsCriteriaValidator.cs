using Dawem.Models.Dtos.Attendances;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Attendances
{
    public class GetEmployeeAssignmentsCriteriaValidator : AbstractValidator<EmployeeGetRequestAssignmentsCriteria>
    {
        public GetEmployeeAssignmentsCriteriaValidator()
        {
            RuleFor(model => model.Month).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustEnterTheMonth);
            RuleFor(model => model.Year).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustEnterTheYear);
        }
    }
}
