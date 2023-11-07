using Dawem.Enums.General;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.WeekAttendances.WeekAttendances
{
    public class UpdateScheduleDaysValidator : AbstractValidator<ScheduleDayUpdateModel>
    {
        public UpdateScheduleDaysValidator()
        {
            RuleFor(model => model.Id)
                .GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustEnterValidId);

            var weekDaysList = Enum.GetValues(typeof(WeekDays)).Cast<WeekDays>().ToList();

            RuleFor(model => model.WeekDay)
                .Must(weekDaysList.Contains)
                .WithMessage(LeillaKeys.SorryYouMustEnterValidWeekDay);

            RuleFor(model => model.ShiftId)
                .Must(s => s > 0)
                .When(s => s.ShiftId != null)
                .WithMessage(LeillaKeys.SorryYouMustChooseValidShift);
        }
    }

}
