using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation
{
    public class GetEmployeeAttendancesCriteriaValidator : AbstractValidator<GetEmployeeAttendancesCriteria>
    {
        public GetEmployeeAttendancesCriteriaValidator()
        {
            RuleFor(model => model.Month).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustEnterTheMonth);
            RuleFor(model => model.Year).GreaterThan(0).
                   WithMessage(LeillaKeys.SorryYouMustEnterTheYear);
        }
    }
}
