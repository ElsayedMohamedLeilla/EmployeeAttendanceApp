using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core
{
    public class UpdateResponsibilityModelValidator : AbstractValidator<UpdateResponsibilityModel>
    {
        public UpdateResponsibilityModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterResponsibilityId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterResponsibilityName);

        }
    }
}
