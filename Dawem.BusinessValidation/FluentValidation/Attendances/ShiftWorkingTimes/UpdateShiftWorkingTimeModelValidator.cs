using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Core.ShiftWorkingTimes
{
    public class UpdateShiftWorkingTimeModelValidator : AbstractValidator<UpdateShiftWorkingTimeModelDTO>
    {
        public UpdateShiftWorkingTimeModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(DawemKeys.SorryYouMustEnterShiftWorkingTimeId);

            RuleFor(model => model.Name).NotNull().
                   WithMessage(DawemKeys.SorryYouMustEnterShiftWorkingTimeName);

            RuleFor(model => model.CheckInTime).GreaterThan(default(DateTime)).
               WithMessage(DawemKeys.SorryYouMustEnterShiftWorkingTimeCheckInTime);

            RuleFor(model => model.CheckOutTime).GreaterThan(default(DateTime)).
                WithMessage(DawemKeys.SorryYouMustEnterShiftWorkingTimeCheckOutTime);

            RuleFor(model => model.CheckInTime).GreaterThan(model => model.CheckOutTime).
              WithMessage(DawemKeys.SorryShiftCheckInTimeCantBeGreaterThanShiftCheckOutTime);

            RuleFor(model => model.AllowedMinutes).LessThan(0).
              WithMessage(DawemKeys.SorryShiftWorkingTimeAllowedMinutesCantBeLessThanZero);

        }
    }
}
