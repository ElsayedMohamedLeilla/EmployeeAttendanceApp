using Dawem.Models.Dtos.Core.PermissionsTypes;
using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Validation.FluentValidation.Core.PermissionsTypes
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
