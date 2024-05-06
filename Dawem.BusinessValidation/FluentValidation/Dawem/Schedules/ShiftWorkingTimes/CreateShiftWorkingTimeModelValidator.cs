using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Schedules.ShiftWorkingTimes
{
    public class CreateShiftWorkingTimeModelValidator : AbstractValidator<CreateShiftWorkingTimeModelDTO>
    {
        public CreateShiftWorkingTimeModelValidator()
        {
            RuleFor(model => model.Name).NotNull().
                   WithMessage(LeillaKeys.SorryYouMustEnterShiftWorkingTimeName);

            RuleFor(model => model.TimePeriod)
               .IsInEnum().WithMessage(AmgadKeys.SorryYouMustChooseValidTimePeriod0Or1);

            RuleFor(model => model.CheckInTime)
                   .Custom((CheckInTime, context) =>
                   {
                       // Check if the TimeSpan is valid (e.g., not negative, within a valid range)
                       if (CheckInTime < TimeSpan.Zero || CheckInTime >= TimeSpan.FromDays(1))
                       {
                           context.AddFailure(AmgadKeys.SorryThisTimeFormatNotValid);
                       }
                   });


            RuleFor(model => model.CheckOutTime)
           .Custom((CheckOutTime, context) =>
           {
               // Check if the TimeSpan is valid (e.g., not negative, within a valid range)
               if (CheckOutTime < TimeSpan.Zero || CheckOutTime >= TimeSpan.FromDays(1))
               {
                   context.AddFailure(AmgadKeys.SorryThisTimeFormatNotValid);
               }
           });


            RuleFor(model => model.AllowedMinutes).GreaterThan(0).
              WithMessage(LeillaKeys.SorryShiftWorkingTimeAllowedMinutesCantBeLessThanZero);


        }
    }
}
