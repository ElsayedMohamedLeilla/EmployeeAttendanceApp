using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Validation.FluentValidation.Core.VacationsTypes
{
    public class UpdateVacationsTypeModelValidator : AbstractValidator<UpdateVacationsTypeDTO>
    {
        public UpdateVacationsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterVacationsTypeName);

        }
    }
}
