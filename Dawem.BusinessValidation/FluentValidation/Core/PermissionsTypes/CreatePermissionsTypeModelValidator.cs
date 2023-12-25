using Dawem.Models.Dtos.Core.PermissionsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.PermissionsTypes
{
    public class CreatePermissionsTypeModelValidator : AbstractValidator<CreatePermissionTypeDTO>
    {
        public CreatePermissionsTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterPermissionsTypeName);

        }
    }
}
