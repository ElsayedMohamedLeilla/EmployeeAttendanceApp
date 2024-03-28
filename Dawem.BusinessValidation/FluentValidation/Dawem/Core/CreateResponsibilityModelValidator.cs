using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.JustificationsTypes
{
    public class CreateResponsibilityModelValidator : AbstractValidator<CreateResponsibilityModel>
    {
        public CreateResponsibilityModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterResponsibilityName);

        }
    }
}
