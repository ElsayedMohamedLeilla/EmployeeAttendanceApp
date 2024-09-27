using Dawem.Helpers;
using Dawem.Models.Dtos.Dawem.Subscriptions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.AdminPanel.Subscriptions
{
    public class AcceptSubscriptionModelValidator : AbstractValidator<AcceptSubscriptionModel>
    {
        public AcceptSubscriptionModelValidator()
        {
            RuleFor(model => model.SubscriptionId).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionId);

            RuleFor(model => model.ActivationStartDate).
                Must(n => n != default).
                WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionActivationStartDate);

        }
    }
}
