using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultPermissionsTypes
{
    public class UpdateDefaultPermissionsTypeModelValidator : AbstractValidator<UpdateDefaultPermissionsTypeDTO>
    {
        public UpdateDefaultPermissionsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(AmgadKeys.SorryYouMustEnterPermissionsTypeId);

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterPermissionsTypeName);

            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectPermissionType);

        }
    }
}
