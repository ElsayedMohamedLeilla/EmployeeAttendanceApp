using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Employees
{
    public class CreateEmployeeModelValidator : AbstractValidator<CreateEmployeeModel>
    {
        public CreateEmployeeModelValidator()
        {
            RuleFor(model => model.DepartmentId).GreaterThan(0).
                    WithMessage(DawemKeys.SorryYouMustChooseDepartment);
            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterEmployeeName);
            RuleFor(model => model.JoiningDate).GreaterThan(default(DateTime)).
                   WithMessage(DawemKeys.SorryYouMustEnterEmployeeJoiningDate);

            RuleFor(model => model.ProfileImageFile)
                  .Must(file => file.Length > 0 && file.ContentType.Contains(DawemKeys.Image))
                  .When(file => file != null)
                  .WithMessage(DawemKeys.SorryYouMustUploadImagesOnly);
        }
    }
}
