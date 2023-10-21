using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Authentication
{
    public class CreateDepartmentModelValidator : AbstractValidator<CreateDepartmentModel>
    {
        public CreateDepartmentModelValidator()
        {
          
            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterDepartmentName);
          
        }
    }
}
