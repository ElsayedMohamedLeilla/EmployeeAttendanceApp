using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Schedules.SchedulePlans
{
    public class CreateSchedulePlanValidator : AbstractValidator<CreateSchedulePlanModel>
    {
        public CreateSchedulePlanValidator()
        {
            RuleFor(model => model.SchedulePlanType)
                .IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterValidSchedulePlanType);

            RuleFor(model => model.EmployeeId)
                .Must(s => s > 0)
                .When(s => s.SchedulePlanType == ForType.Employees)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployeeWhenSchedulePlanTypeIsEmployee);

            RuleFor(model => model.GroupId)
               .Must(s => s > 0)
               .When(s => s.SchedulePlanType == ForType.Groups)
               .WithMessage(LeillaKeys.SorryYouMustChooseGroupWhenSchedulePlanTypeIsGroup);

            RuleFor(model => model.DepartmentId)
               .Must(s => s > 0)
               .When(s => s.SchedulePlanType == ForType.Departments)
               .WithMessage(LeillaKeys.SorryYouMustChooseDepartmentWhenSchedulePlanTypeIsDepartment);

            RuleFor(model => model.ScheduleId)
                .Must(s => s > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseSchedule);

            RuleFor(model => model.DateFrom)
                .Must(BeAValidDate)
                .WithMessage(LeillaKeys.SorryYouMustEnterAValidDateFrom);

            RuleFor(model => model.DateFrom)
                .Must(d => d > DateTime.UtcNow)
                .WithMessage(LeillaKeys.SorryDateFromMustBiggerThanToday);
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default);
        }
    }

}
