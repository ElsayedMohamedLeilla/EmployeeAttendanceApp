using AutoMapper;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.DTOs.Dawem.Screens.Screens;

namespace Dawem.Models.AutoMapper.Dawem.Subscriptions
{
    public class ScreenMapProfile : Profile
    {
        public ScreenMapProfile()
        {
            CreateMap<CreateScreenModel, MenuItem>().
                ForMember(dest => dest.MenuItemNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));
            CreateMap<UpdateScreenModel, MenuItem>().
                ForMember(dest => dest.MenuItemNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));


            CreateMap<NameTranslationModel, MenuItemNameTranslation>();
        }
    }
}