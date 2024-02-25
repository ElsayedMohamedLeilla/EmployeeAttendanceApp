using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Employees
{
    public class AttendanceSummaryCritriaValidator : AbstractValidator<AttendanceSummaryCritria>
    {
        public AttendanceSummaryCritriaValidator()
        {
            RuleFor(model => model.DateFrom)
                .Must(d => d != default)
                .WithMessage(LeillaKeys.SorryYouMustEnterAValidDateFrom);

            RuleFor(model => model.DateTo)
                .Must(d => d != default)
                .WithMessage(LeillaKeys.SorryYouMustEnterAValidDateTo);

            RuleFor(model => model.DateTo)
                .Must(dateTo => dateTo <= DateTime.UtcNow.Date)
                .WithMessage(LeillaKeys.SorryDateToMustLessThanOrEqualTodayDay);

            RuleFor(model => model)
                .Must(m => m.DateTo >= m.DateFrom)
                .WithMessage(LeillaKeys.SorryDateToMustGreaterThanOrEqualDateFrom);

            RuleFor(model => model)
                .Must(m => m.DateFrom.AddMonths(3) >= m.DateTo)
                .WithMessage(LeillaKeys.SorryPeriodMustLessThanOrEqualThreeMonths);
        }
    }
}
