using AutoMapper;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Subscriptions;

namespace Dawem.Models.AutoMapper.Subscriptions
{
    public class SubscriptionsMapProfile : Profile
    {
        public SubscriptionsMapProfile()
        {
            CreateMap<CreateSubscriptionModel, Subscription>();
            CreateMap<UpdateSubscriptionModel, Subscription>();

        }
    }
}
