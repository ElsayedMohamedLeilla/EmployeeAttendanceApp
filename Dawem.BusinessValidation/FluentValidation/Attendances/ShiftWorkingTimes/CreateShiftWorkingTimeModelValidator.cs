using Dawem.Helpers;
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

            RuleFor(model => model.TimePeriod)
               .IsInEnum().WithMessage(AmgadKeys.SorryYouMustChooseValidTimePeriod0Or1);

            RuleFor(model => model.CheckInTime)
                   .Custom((CheckInTime, context) =>
                   {
                       // Convert TimeOnly to TimeSpan
                       var timeSpan = TimeOnlyHelper.ToTimeSpan(CheckInTime);

                       // Check if the TimeSpan is valid (e.g., not negative, within a valid range)
                       if (timeSpan < TimeSpan.Zero || timeSpan >= TimeSpan.FromDays(1))
                       {
                           context.AddFailure(AmgadKeys.SorryThisTimeFormatNotValid);
                       }
                   });


            RuleFor(model => model.CheckOutTime)
           .Custom((CheckOutTime, context) =>
           {
               // Convert TimeOnly to TimeSpan
               var timeSpan = TimeOnlyHelper.ToTimeSpan(CheckOutTime);

               // Check if the TimeSpan is valid (e.g., not negative, within a valid range)
               if (timeSpan < TimeSpan.Zero || timeSpan >= TimeSpan.FromDays(1))
               {
                   context.AddFailure(AmgadKeys.SorryThisTimeFormatNotValid);
               }
           });


            RuleFor(model => model.AllowedMinutes).GreaterThan(0).
              WithMessage(LeillaKeys.SorryShiftWorkingTimeAllowedMinutesCantBeLessThanZero);


        }
    }
}
