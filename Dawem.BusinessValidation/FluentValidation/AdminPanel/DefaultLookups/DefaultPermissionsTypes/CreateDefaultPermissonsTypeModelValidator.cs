using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultPermissionsTypes
{
    public class CreateDefaultPermissionsTypeModelValidator : AbstractValidator<CreateDefaultPermissionsTypeDTO>
    {
        public CreateDefaultPermissionsTypeModelValidator()
        {

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterPermissionsTypeName);
            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectPermissionType);
        }
    }
}
