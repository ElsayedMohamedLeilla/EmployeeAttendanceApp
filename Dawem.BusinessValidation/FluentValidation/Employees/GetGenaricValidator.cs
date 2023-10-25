using Dawem.Models.Criteria;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees
{
    public class GetGenaricValidator : AbstractValidator<BaseCriteria>
    {
        public GetGenaricValidator()
        {
            RuleFor(model => model).Must(m => m.PagingEnabled && m.PageSize < 20).
                    WithMessage(DawemKeys.SorryYouMustEnterPaginationSettings);
        }
    }
}
