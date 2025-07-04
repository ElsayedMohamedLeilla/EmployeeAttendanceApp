﻿using Dawem.Models.Criteria;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation
{
    public class GetGenaricValidator : AbstractValidator<BaseCriteria>
    {
        public GetGenaricValidator()
        {
            RuleFor(model => model).Must(m => m.PagingEnabled)
                .When(m => !m.IsExport)
                .WithMessage(LeillaKeys.SorryYouMustEnablePagination);

            RuleFor(model => model)
                .Must(m => m.PageSize <= 5)
                .When(m => !m.IsExport)
                .WithMessage(LeillaKeys.SorryPageSizeMustLessThanOrEqual5);
        }
    }
}
