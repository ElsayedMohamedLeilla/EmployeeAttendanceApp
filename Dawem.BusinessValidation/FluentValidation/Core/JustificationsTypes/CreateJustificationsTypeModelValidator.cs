using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Translations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Validation.FluentValidation.Core.JustificationsTypes
{
    public class CreateJustificationsTypeModelValidator : AbstractValidator<CreateJustificationsTypeDTO>
    {
        public CreateJustificationsTypeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterJustificationsTypeName);

        }
    }
}
