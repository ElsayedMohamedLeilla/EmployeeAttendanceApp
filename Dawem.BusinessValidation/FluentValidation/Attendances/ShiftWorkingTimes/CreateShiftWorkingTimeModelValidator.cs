using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.ShiftWorkingTimes
{
    public class CreateShiftWorkingTimeModelValidator : AbstractValidator<CreateShiftWorkingTimeModelDTO>
    {
        public CreateShiftWorkingTimeModelValidator()
        {

            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterShiftWorkingTimeName);

            RuleFor(model => model.CheckInTime).GreaterThan(default(DateTime)).
                 WithMessage(LeillaKeys.SorryYouMustEnterShiftWorkingTimeCheckInTime);

            RuleFor(model => model.CheckOutTime).GreaterThan(default(DateTime)).
                WithMessage(LeillaKeys.SorryYouMustEnterShiftWorkingTimeCheckOutTime);

            RuleFor(model => model.CheckInTime).GreaterThan(model => model.CheckOutTime).
              WithMessage(LeillaKeys.SorryShiftCheckInTimeCantBeGreaterThanShiftCheckOutTime);

            RuleFor(model => model.AllowedMinutes).LessThan(0).
              WithMessage(LeillaKeys.SorryShiftWorkingTimeAllowedMinutesCantBeLessThanZero);


        }
    }
}
