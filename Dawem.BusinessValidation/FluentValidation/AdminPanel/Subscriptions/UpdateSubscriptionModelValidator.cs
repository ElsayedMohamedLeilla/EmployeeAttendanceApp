﻿using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.Subscriptions
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
                Must(d => d != default).
                WithMessage(LeillaKeys.SorryYouMustEnterEndDate);

            RuleFor(model => model.FollowUpEmail).
               NotNull().
               WithMessage(LeillaKeys.SorryYouMustEnterFollowUpEmail);
        }
    }
}
