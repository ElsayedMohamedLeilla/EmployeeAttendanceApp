using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Attendances.Schedules
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
                .When(s => s.SchedulePlanType == SchedulePlanType.Employees)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployeeWhenSchedulePlanTypeIsEmployee);

            RuleFor(model => model.GroupId)
               .Must(s => s > 0)
               .When(s => s.SchedulePlanType == SchedulePlanType.Groups)
               .WithMessage(LeillaKeys.SorryYouMustChooseGroupWhenSchedulePlanTypeIsGroup);

            RuleFor(model => model.DepartmentId)
               .Must(s => s > 0)
               .When(s => s.SchedulePlanType == SchedulePlanType.Departments)
               .WithMessage(LeillaKeys.SorryYouMustChooseDepartmentWhenSchedulePlanTypeIsDepartment);

            RuleFor(model => model.ScheduleId)
                .Must(s => s > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseSchedule);

            RuleFor(model => model.DateFrom)
                .Must(BeAValidDate)
                .WithMessage(LeillaKeys.SorryYouMustEnterAValidDateFrom);

            RuleFor(model => model.DateFrom)
                .Must(d=> d > DateTime.UtcNow)
                .WithMessage(LeillaKeys.SorryDateFromMustBiggerThanToday);
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default);
        }
    }

}
