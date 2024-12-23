using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.VacationsTypes
{
    public class UpdateOvertimesTypeModelValidator : AbstractValidator<UpdateOvertimeTypeDTO>
    {
        public UpdateOvertimesTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);

        }
    }
}
