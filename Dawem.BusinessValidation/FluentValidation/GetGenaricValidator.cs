using Dawem.Models.Criteria;
using Dawem.Models.Dtos.Employees.Department;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation
{
    public class GetGenaricValidator : AbstractValidator<BaseCriteria>
    {
        public GetGenaricValidator()
        {
            RuleFor(model => model).Must(m => m.PagingEnabled).
                    WithMessage(LeillaKeys.SorryYouMustEnablePagination);

            RuleFor(model => model).Must(m => m.PageSize <= 5).
                    WithMessage(LeillaKeys.SorryPageSizeMustLessThanOrEqual5);
        }
    }
}
