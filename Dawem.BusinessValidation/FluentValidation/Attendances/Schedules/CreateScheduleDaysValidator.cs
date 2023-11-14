using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Attendances.Schedules;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Attendances.Schedules
{
    public class CreateScheduleDaysValidator : AbstractValidator<ScheduleDayCreateModel>
    {
        public CreateScheduleDaysValidator()
        {
            var weekDaysList = Enum.GetValues(typeof(WeekDay)).Cast<WeekDay>().ToList();

            RuleFor(model => model.WeekDay)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterValidWeekDay);

            RuleFor(model => model.ShiftId)
                .Must(s => s > 0)
                .When(s => s.ShiftId != null)
                .WithMessage(LeillaKeys.SorryYouMustChooseValidShift);
        }
    }

}
