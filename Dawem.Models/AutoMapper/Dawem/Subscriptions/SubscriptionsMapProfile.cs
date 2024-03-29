using AutoMapper;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Dawem.Subscriptions;

namespace Dawem.Models.AutoMapper.Dawem.Subscriptions
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
