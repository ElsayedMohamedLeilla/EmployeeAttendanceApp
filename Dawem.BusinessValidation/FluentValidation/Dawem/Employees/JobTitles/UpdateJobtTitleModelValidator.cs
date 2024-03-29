using Dawem.Models.Dtos.Dawem.Employees.JobTitles;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.JobTitles
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
