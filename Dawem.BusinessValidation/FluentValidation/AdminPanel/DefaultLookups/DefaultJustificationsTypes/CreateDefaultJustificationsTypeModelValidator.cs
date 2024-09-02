using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultJustificationsTypes
{
    public class CreateDefaultJustificationsTypeModelValidator : AbstractValidator<CreateDefaultJustificationsTypeDTO>
    {
        public CreateDefaultJustificationsTypeModelValidator()
        {

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterJustificationsTypeName);
            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectJustificationType);
        }
    }
}
