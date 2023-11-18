using Dawem.Helpers;
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
                    WithMessage(LeillaKeys.SorryYouMustChooseDepartment);
            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterEmployeeName);
            RuleFor(model => model.JoiningDate).GreaterThan(default(DateTime)).
                   WithMessage(LeillaKeys.SorryYouMustEnterEmployeeJoiningDate);
            RuleFor(model => model.Email).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterEmail);
            RuleFor(model => model.MobileNumber).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterMobileNumber);
            RuleFor(model => model.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);
            RuleFor(model => model.AttendanceType).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterAttendanceType);

            RuleFor(model => model.ProfileImageFile)
                  .Must(file => file.Length > 0 && file.ContentType.Contains(LeillaKeys.Image))
                  .When(file => file != null)
                  .WithMessage(LeillaKeys.SorryYouMustUploadImagesOnly);
        }
    }
}
