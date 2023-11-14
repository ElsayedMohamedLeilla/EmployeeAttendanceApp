using Dawem.Models.Dtos.Attendances.Schedules;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Attendances.Schedules
{
    public class CreateScheduleModelValidator : AbstractValidator<CreateScheduleModel>
    {
        public CreateScheduleModelValidator()
        {
            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterScheduleName);

            RuleFor(model => model.ScheduleDays)
                .NotNull()
                .WithMessage(LeillaKeys.SorryYouMustEnterSchedules)
                .Must(w => w != null && w.Count == 7)
                .When(w => w != null)
                .WithMessage(LeillaKeys.SorryYouMustEnterAll7WeekDaysInScheduleDays);

            RuleFor(model => model.ScheduleDays)
               .Must(w => w != null && !w.GroupBy(x => x.WeekDay).ToList().Any(w => w.Count() > 1))
               .When(w => w != null)
               .WithMessage(LeillaKeys.SorryWeekDayCannotBeRepeated);

            RuleForEach(model => model.ScheduleDays).SetValidator(new CreateScheduleDaysValidator());
        }
    }
}
