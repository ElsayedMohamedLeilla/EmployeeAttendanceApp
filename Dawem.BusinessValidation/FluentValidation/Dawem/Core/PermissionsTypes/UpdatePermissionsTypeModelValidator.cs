using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.PermissionsTypes
{
    public class UpdatePermissionsTypeModelValidator : AbstractValidator<UpdatePermissionTypeDTO>
    {
        public UpdatePermissionsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterPermissionsTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterPermissionsTypeName);

        }
    }
}
