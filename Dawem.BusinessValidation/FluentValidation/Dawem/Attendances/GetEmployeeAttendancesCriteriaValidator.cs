using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Attendances
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
