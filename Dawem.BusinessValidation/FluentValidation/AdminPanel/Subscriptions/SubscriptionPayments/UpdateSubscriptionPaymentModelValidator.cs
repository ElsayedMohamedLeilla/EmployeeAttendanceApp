using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Subscriptions.Plans
{
    public class UpdateSubscriptionPaymentModelValidator : AbstractValidator<UpdateSubscriptionPaymentModel>
    {
        public UpdateSubscriptionPaymentModelValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionPaymentId);

            RuleFor(model => model.Amount).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionPaymentAmount);

            RuleFor(model => model.SubscriptionId).
                Must(n => n > 0).
                WithMessage(LeillaKeys.SorryYouMustChooseSubscription);

            RuleFor(model => model.Date).
                Must(n => n != default).
                WithMessage(LeillaKeys.SorryYouMustEnterSubscriptionPaymentDate);
        }
    }
}
