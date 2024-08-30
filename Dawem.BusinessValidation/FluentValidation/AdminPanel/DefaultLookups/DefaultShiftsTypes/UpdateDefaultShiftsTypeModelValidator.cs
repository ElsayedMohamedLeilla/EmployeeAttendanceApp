using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultVacationsTypes
{
    public class UpdateDefaultShiftsTypeModelValidator : AbstractValidator<UpdateDefaultShiftsTypeDTO>
    {
        public UpdateDefaultShiftsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeId);

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);

            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);

        }
    }
}
