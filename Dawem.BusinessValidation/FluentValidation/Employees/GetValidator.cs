using Dawem.Models.Criteria;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees
{
    public class GetValidator : AbstractValidator<BaseCriteria>
    {
        public GetValidator()
        {
            RuleFor(model => model).Must(m => m.PagingEnabled && m.PageSize < 20).
                    WithMessage(DawemKeys.SorryYouMustEnterPaginationSettings);
        }
    }
}
