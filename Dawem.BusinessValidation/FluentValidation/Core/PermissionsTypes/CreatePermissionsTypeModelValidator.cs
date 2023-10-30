using Dawem.Models.Dtos.Core.PermissionsTypes;
using Dawem.Translations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Validation.FluentValidation.Core.PermissionsTypes
{
    public class CreatePermissionsTypeModelValidator : AbstractValidator<CreatePermissionsTypeDTO>
    {
        public CreatePermissionsTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterPermissionsTypeName);

        }
    }
}
