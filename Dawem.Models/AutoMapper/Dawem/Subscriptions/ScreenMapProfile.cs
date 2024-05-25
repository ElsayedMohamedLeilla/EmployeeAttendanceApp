using AutoMapper;
using Dawem.Domain.Entities.Others;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.DTOs.Dawem.Screens.Screens;

namespace Dawem.Models.AutoMapper.Dawem.Subscriptions
{
    public class ScreenMapProfile : Profile
    {
        public ScreenMapProfile()
        {
            CreateMap<CreateScreenModel, Screen>().
                ForMember(dest => dest.ScreenNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));
            CreateMap<UpdateScreenModel, Screen>().
                ForMember(dest => dest.ScreenNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));


            CreateMap<NameTranslationModel, ScreenNameTranslation>();
        }
    }
}