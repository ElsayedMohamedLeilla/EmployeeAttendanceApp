using Dawem.Models.Dtos.Requests.Assignments;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Requests.Assignments
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
