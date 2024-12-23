using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.VacationsTypes
{
    public class CreateOvertimesTypeModelValidator : AbstractValidator<CreateOvertimeTypeDTO>
    {
        public CreateOvertimesTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);
        }
    }
}
