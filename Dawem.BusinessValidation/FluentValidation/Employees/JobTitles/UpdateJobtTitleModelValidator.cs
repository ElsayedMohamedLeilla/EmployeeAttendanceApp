using Dawem.Models.Dtos.Employees.JobTitles;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.JobTitles
{
    public class UpdateJobtTitleModelValidator : AbstractValidator<UpdateJobTitleModel>
    {
        public UpdateJobtTitleModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterJobTitleId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterJobTitleName);

        }
    }
}
