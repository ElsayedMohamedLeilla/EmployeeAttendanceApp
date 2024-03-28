using Dawem.Models.Dtos.Dawem.Core.JustificationsTypes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Core.JustificationsTypes
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
