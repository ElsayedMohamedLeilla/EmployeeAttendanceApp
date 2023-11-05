using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Translations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Validation.FluentValidation.Core.VacationsTypes
{
    public class CreateVacationsTypeModelValidator : AbstractValidator<CreateVacationsTypeDTO>
    {
        public CreateVacationsTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);

        }
    }
}
