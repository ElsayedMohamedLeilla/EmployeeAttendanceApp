using AutoMapper;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Dawem.Subscriptions.SubscriptionPayment;

namespace Dawem.Models.AutoMapper.Dawem.Subscriptions
{
    public class SubscriptionPaymentsMapProfile : Profile
    {
        public SubscriptionPaymentsMapProfile()
        {
            CreateMap<CreateSubscriptionPaymentModel, SubscriptionPayment>();
            CreateMap<UpdateSubscriptionPaymentModel, SubscriptionPayment>();
        }
    }
}
