using Dawem.Models.Dtos.Dawem.Employees.JobTitles;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Employees.JobTitles
{
    public class CreateJobTitleModelValidator : AbstractValidator<CreateJobTitleModel>
    {
        public CreateJobTitleModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterJobTitleName);

        }
    }
}
