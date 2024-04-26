using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Attendances
{
    public class GetEmployeeSchedulesModelValidator : AbstractValidator<GetEmployeeSchedulesModel>
    {
        public GetEmployeeSchedulesModelValidator()
        {
            RuleFor(model => model.DateFrom).
                Must(d=> d != default).
                WithMessage(LeillaKeys.SorryYouMustEnterAValidDateFrom);

            RuleFor(model => model.DateTo).
                Must(d => d != default).
                WithMessage(LeillaKeys.SorryYouMustEnterAValidDateTo);

            RuleFor(model => model).
                Must(d => d.DateTo >= d.DateFrom).
                WithMessage(LeillaKeys.SorryDateToMustGreaterThanOrEqualDateFrom);

            //RuleFor(model => model).
            //    Must(d => (d.DateTo - d.DateFrom).TotalDays <= 30).
            //    WithMessage(LeillaKeys.SorryPeriodCanNotGreaterThan30Day);
        }
    }
}
