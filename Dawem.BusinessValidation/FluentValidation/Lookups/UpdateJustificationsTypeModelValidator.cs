using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Validation.FluentValidation.Lookups
{
    public class UpdateJustificationsTypeModelValidator : AbstractValidator<UpdateJustificationsTypeDTO>
    {
        public UpdateJustificationsTypeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(DawemKeys.SorryYouMustEnterJustificationsTypeId);
          
            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterJustificationsTypeName);
           
        }
    }
}
