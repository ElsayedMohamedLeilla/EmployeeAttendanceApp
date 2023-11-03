using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.WeekAttendances.WeekAttendances
{
    public class UpdateWeekAttendanceModelValidator : AbstractValidator<UpdateWeekAttendanceModel>
    {
        public UpdateWeekAttendanceModelValidator()
        {
            RuleFor(model => model.Id).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterValidId);


            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterWeekAttendanceName);

            RuleFor(model => model.WeekShifts)
                .NotNull()
                .WithMessage(DawemKeys.SorryYouMustEnterWeekShifts)
                .Must(w => w.Count == 7)
                .WithMessage(DawemKeys.SorryYouMustEnterAll7WeekDaysInWeekShifts);

            RuleFor(model => model.WeekShifts)
                .Must(w => !w.GroupBy(x => x.WeekDay).ToList().Any(w => w.Count() > 1))
                .WithMessage(DawemKeys.SorryWeekDayCannotBeRepeated);

            RuleForEach(model => model.WeekShifts).SetValidator(new UpdateWeekShiftsValidator());
        }
    }
}
