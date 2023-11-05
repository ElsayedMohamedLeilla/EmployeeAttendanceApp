using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.WeekAttendances.WeekAttendances
{
    public class CreateWeekAttendanceModelValidator : AbstractValidator<CreateWeekAttendanceModel>
    {
        public CreateWeekAttendanceModelValidator()
        {
            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterWeekAttendanceName);

            RuleFor(model => model.WeekShifts)
                .NotNull()
                .WithMessage(LeillaKeys.SorryYouMustEnterWeekShifts)
                .Must(w => w != null && w.Count == 7)
                .When(w => w != null)
                .WithMessage(LeillaKeys.SorryYouMustEnterAll7WeekDaysInWeekShifts);

            RuleFor(model => model.WeekShifts)
               .Must(w => w != null && !w.GroupBy(x => x.WeekDay).ToList().Any(w => w.Count() > 1))
               .When(w => w != null)
               .WithMessage(LeillaKeys.SorryWeekDayCannotBeRepeated);

            RuleForEach(model => model.WeekShifts).SetValidator(new CreateWeekShiftsValidator());
        }
    }
}
