using AutoMapper;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Dtos.Subscriptions.Plans;

namespace Dawem.Models.AutoMapper.Subscriptions
{
    public class PlansMapProfile : Profile
    {
        public PlansMapProfile()
        {
            CreateMap<CreatePlanModel, Plan>().
                ForMember(dest => dest.PlanNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));
            CreateMap<UpdatePlanModel, Plan>().
                ForMember(dest => dest.PlanNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));


            CreateMap<NameTranslationModel, PlanNameTranslation>();
        }
    }
}
