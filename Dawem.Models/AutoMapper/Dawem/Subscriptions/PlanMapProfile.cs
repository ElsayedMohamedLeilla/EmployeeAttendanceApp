using AutoMapper;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;

namespace Dawem.Models.AutoMapper.Dawem.Subscriptions
{
    public class PlanMapProfile : Profile
    {
        public PlanMapProfile()
        {
            CreateMap<CreatePlanModel, Plan>().
                ForMember(dest => dest.PlanNameTranslations, opt => opt.MapFrom(src => src.NameTranslations))
                .AfterMap(MapPlanScreens); ;
            CreateMap<UpdatePlanModel, Plan>().
                ForMember(dest => dest.PlanNameTranslations, opt => opt.MapFrom(src => src.NameTranslations))
                .AfterMap(MapPlanScreens); ;


            CreateMap<NameTranslationModel, PlanNameTranslation>();
        }
        private void MapPlanScreens(CreatePlanModel source, Plan destination, ResolutionContext context)
        {
            destination.PlanScreens = source.ScreenIds
                .Select(screenId => new PlanScreen { ScreenId = screenId })
                .ToList();
        }
        private void MapPlanScreens(UpdatePlanModel source, Plan destination, ResolutionContext context)
        {
            destination.PlanScreens = source.ScreenIds
                .Select(screenId => new PlanScreen { ScreenId = screenId })
                .ToList();
        }
    }
}