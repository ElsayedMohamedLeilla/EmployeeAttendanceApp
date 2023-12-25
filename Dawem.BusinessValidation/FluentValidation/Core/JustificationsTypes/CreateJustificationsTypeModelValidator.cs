using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Translations;
using FluentValidation;

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
