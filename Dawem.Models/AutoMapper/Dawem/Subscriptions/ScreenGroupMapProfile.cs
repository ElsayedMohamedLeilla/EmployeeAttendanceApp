using AutoMapper;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;

namespace Dawem.Models.AutoMapper.Dawem.Subscriptions
{
    public class ScreenGroupMapProfile : Profile
    {
        public ScreenGroupMapProfile()
        {
            CreateMap<CreateScreenGroupModel, MenuItem>().
                ForMember(dest => dest.MenuItemNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));
            CreateMap<UpdateScreenGroupModel, MenuItem>().
                ForMember(dest => dest.MenuItemNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));


            CreateMap<NameTranslationModel, MenuItemNameTranslation>();
        }
    }
}