using Dawem.Models.Dtos.Employees.Departments;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Employees.Departments
{
    public class UpdateSubscriptionModelValidator : AbstractValidator<UpdateSubscriptionModel>
    {
        public UpdateSubscriptionModelValidator()
        {
            RuleFor(model => model.Id).
                GreaterThan(0).
                WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionId);

            RuleFor(model => model.PlanId).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustChoosePlan);

            RuleFor(model => model.CompanyId).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustChooseCompany);

            RuleFor(model => model.DurationInDays).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterDurationInDays);

            RuleFor(model => model.Status).
                IsInEnum().
                WithMessage(LeillaKeys.SorryYouMustChooseSubscriptionStatus);

            RuleFor(model => model.StartDate).
                Must(d => d != default).
                WithMessage(LeillaKeys.SorryYouMustEnterStartDate);

            RuleFor(model => model.EndDate).
                IsInEnum().
                WithMessage(LeillaKeys.SorryYouMustEnterEndDate);

            RuleFor(model => model.FollowUpEmail).
               IsInEnum().
               WithMessage(LeillaKeys.SorryYouMustEnterFollowUpEmail);
        }
    }
}
