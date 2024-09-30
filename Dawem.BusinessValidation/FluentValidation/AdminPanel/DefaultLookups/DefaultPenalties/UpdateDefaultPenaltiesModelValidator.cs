using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultPenalties
{
    public class UpdateDefaultPenaltiesModelValidator : AbstractValidator<UpdateDefaultPenaltiesDTO>
    {
        public UpdateDefaultPenaltiesModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(AmgadKeys.SorryYouMustEnterPenaltyId);

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterPenaltiesName);

            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);

        }
    }
}
