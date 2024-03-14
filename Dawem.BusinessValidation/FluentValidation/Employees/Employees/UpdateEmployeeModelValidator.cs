using Dawem.Helpers;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Employees
{
    public class UpdateEmployeeModelValidator : AbstractValidator<UpdateEmployeeModel>
    {
        public UpdateEmployeeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterEmployeeId);

            RuleFor(model => model.DepartmentId).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustChooseDepartment);

            RuleFor(model => model.AttendanceType).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterAttendanceType);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterEmployeeName);

            RuleFor(model => model.Email).NotNull().
                WithMessage(LeillaKeys.SorryYouMustEnterEmail);

            RuleFor(model => model.MobileNumber).NotNull().
                 WithMessage(LeillaKeys.SorryYouMustEnterMobileNumber);


            RuleFor(user => user.MobileCountryId)
            .GreaterThan(0)
            .WithMessage(LeillaKeys.SorryYouMustChooseMobileCountry);

            RuleFor(model => model.Email).Must(EmailHelper.IsValidEmail).
                WithMessage(LeillaKeys.SorryYouMustEnterValidEmail);
            RuleFor(model => model.JoiningDate).GreaterThan(default(DateTime)).
                   WithMessage(LeillaKeys.SorryYouMustEnterEmployeeName);
            RuleFor(model => model.AttendanceType).IsInEnum().
                    WithMessage(LeillaKeys.SorryYouMustChooseAttendanceType);
            RuleFor(model => model.EmployeeType).IsInEnum().
                    WithMessage(LeillaKeys.SorryYouMustChooseEmployeeType);
            RuleFor(model => model.ProfileImageFile)
                 .Must(file => file != null && file.Length > 0 && file.ContentType.Contains(LeillaKeys.Image))
                 .When(model => model.ProfileImageFile != null)
                 .WithMessage(LeillaKeys.SorryYouMustUploadImagesOnly);
        }
    }
}
