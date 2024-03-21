using AutoMapper;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Employees.Departments;

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
