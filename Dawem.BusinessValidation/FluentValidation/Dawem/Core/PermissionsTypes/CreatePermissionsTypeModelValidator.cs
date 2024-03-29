using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.PermissionsTypes
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
