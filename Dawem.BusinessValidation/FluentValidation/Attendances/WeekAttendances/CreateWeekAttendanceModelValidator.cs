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
                   WithMessage(DawemKeys.SorryYouMustEnterWeekAttendanceName);

            RuleFor(model => model.WeekShifts)
                .NotNull()
                .WithMessage(DawemKeys.SorryYouMustEnterWeekShifts)
                .Must(w => w != null && w.Count == 7)
                .When(w => w != null)
                .WithMessage(DawemKeys.SorryYouMustEnterAll7WeekDaysInWeekShifts);

            RuleFor(model => model.WeekShifts)
               .Must(w => w != null && !w.GroupBy(x => x.WeekDay).ToList().Any(w => w.Count() > 1))
               .When(w => w != null)
               .WithMessage(DawemKeys.SorryWeekDayCannotBeRepeated);

            RuleForEach(model => model.WeekShifts).SetValidator(new CreateWeekShiftsValidator());
        }
    }
}
