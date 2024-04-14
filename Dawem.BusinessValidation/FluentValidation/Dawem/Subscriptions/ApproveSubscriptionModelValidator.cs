using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Subscriptions
{
    public class ApproveSubscriptionModelValidator : AbstractValidator<ApproveSubscriptionModel>
    {
        public ApproveSubscriptionModelValidator()
        {
            RuleFor(model => model.SubscriptionId).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionId);

            RuleFor(model => model.ActivationStartDate).
                Must(n => default).
                WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionActivationStartDate);

        }
    }
}
