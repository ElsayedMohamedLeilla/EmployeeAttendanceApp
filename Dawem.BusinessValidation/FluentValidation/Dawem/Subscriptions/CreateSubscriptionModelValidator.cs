using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Subscriptions
{
    public class CreateSubscriptionModelValidator : AbstractValidator<CreateSubscriptionModel>
    {
        public CreateSubscriptionModelValidator()
        {
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
                Must(d => d != default).
                WithMessage(LeillaKeys.SorryYouMustEnterEndDate);

            RuleFor(model => model).
                Must(d => d.StartDate <= d.EndDate).
                WithMessage(LeillaKeys.SorryStartDateMustLessThanOrEqualEndDate);

            RuleFor(model => model.FollowUpEmail).
               NotNull().
               WithMessage(LeillaKeys.SorryYouMustEnterFollowUpEmail);

            RuleFor(model => model.FollowUpEmail).
               Must(EmailHelper.IsValidEmail).
               WithMessage(LeillaKeys.SorryYouMustEnterValidFollowUpEmail);

            RuleFor(model => model).
               Must(model => model.DurationInDays == (model.EndDate - model.StartDate).TotalDays).
               WithMessage(LeillaKeys.SorryDurationInDaysMustEqualPeriodBetweenStartDateAndEndDate);

        }
    }
}
