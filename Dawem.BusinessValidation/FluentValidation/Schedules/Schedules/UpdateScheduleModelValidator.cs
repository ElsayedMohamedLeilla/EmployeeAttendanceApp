using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Schedules.Schedules
{
    public class UpdateScheduleModelValidator : AbstractValidator<UpdateScheduleModel>
    {
        public UpdateScheduleModelValidator()
        {
            RuleFor(model => model.Id).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterValidId);


            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterScheduleName);

            RuleFor(model => model.ScheduleDays)
                .NotNull()
                .WithMessage(LeillaKeys.SorryYouMustEnterScheduleDays)
                .Must(w => w.Count == 7)
                .WithMessage(LeillaKeys.SorryYouMustEnterAll7WeekDaysInScheduleDays);

            RuleFor(model => model.ScheduleDays)
                .Must(w => !w.GroupBy(x => x.WeekDay).ToList().Any(w => w.Count() > 1))
                .WithMessage(LeillaKeys.SorryWeekDayCannotBeRepeated);

            RuleForEach(model => model.ScheduleDays).SetValidator(new UpdateScheduleDaysValidator());
        }
    }
}
