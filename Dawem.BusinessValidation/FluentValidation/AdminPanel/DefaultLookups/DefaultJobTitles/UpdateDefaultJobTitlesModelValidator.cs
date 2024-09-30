using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultJobTitles
{
    public class UpdateDefaultJobTitlesModelValidator : AbstractValidator<UpdateDefaultJobTitlesDTO>
    {
        public UpdateDefaultJobTitlesModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterJobTitleId);

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterJobTitlesName);

            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);

        }
    }
}
