using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultJobTitles
{
    public class CreateDefaultJobTitlesModelValidator : AbstractValidator<CreateDefaultJobTitlesDTO>
    {
        public CreateDefaultJobTitlesModelValidator()
        {

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterJobTitlesName);
            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);
        }
    }
}
