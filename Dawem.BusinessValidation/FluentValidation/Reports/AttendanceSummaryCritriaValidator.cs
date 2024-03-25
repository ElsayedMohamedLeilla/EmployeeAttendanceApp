using Dawem.Models.Dtos.Reports.AttendanceSummaryReport;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Reports
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

            RuleFor(model => model).
                Must(m => m.FilterTypeFrom != null || m.FilterTypeTo != null).
                When(m => m.FilterType != null).
                WithMessage(LeillaKeys.SorryYouMustEnterFilterTypeFromOrFilterTypeToWhenChooseFilterType);

            RuleFor(model => model).
                Must(m => m.FilterTypeFrom <= m.FilterTypeTo).
                When(m => m.FilterTypeFrom != null && m.FilterTypeTo != null).
                WithMessage(LeillaKeys.SorryFilterTypeFromMustLessThanOrEqualFilterTypeTo);
        }
    }
}
