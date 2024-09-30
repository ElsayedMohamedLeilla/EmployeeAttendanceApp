using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.DefaultLookups.DefaultDepartments
{
    public class CreateDefaultDepartmentsModelValidator : AbstractValidator<CreateDefaultDepartmentsDTO>
    {
        public CreateDefaultDepartmentsModelValidator()
        {

            //RuleFor(model => model.Name).NotNull().
            //       WithMessage(LeillaKeys.SorryYouMustEnterDepartmentsName);
            //RuleFor(model => model.DefaultType).IsInEnum().
            //       WithMessage(LeillaKeys.SorryYouMustEnterCorrectVacationType);
        }
    }
}
