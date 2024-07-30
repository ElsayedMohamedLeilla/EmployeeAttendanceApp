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
            CreateMap<CreatePlanModel, Plan>().AfterMap(MapPlanScreens);
            CreateMap<UpdatePlanModel, Plan>().AfterMap(MapPlanScreens);

            CreateMap<NameTranslationModel, PlanNameTranslation>();
        }
        private void MapPlanScreens(CreatePlanModel source, Plan destination, ResolutionContext context)
        {
            destination.PlanScreens = source.ScreensIds != null ? source.ScreensIds
                .Select(screenId => new PlanScreen { ScreenId = screenId })
                .ToList() : null;
        }
        private void MapPlanScreens(UpdatePlanModel source, Plan destination, ResolutionContext context)
        {
            destination.PlanScreens = source.ScreensIds != null ? source.ScreensIds
                .Select(screenId => new PlanScreen { ScreenId = screenId })
                .ToList() : null;
        }
    }
}