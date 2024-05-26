using AutoMapper;
using Dawem.Domain.Entities.Others;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;

namespace Dawem.Models.AutoMapper.Dawem.Subscriptions
{
    public class ScreenGroupMapProfile : Profile
    {
        public ScreenGroupMapProfile()
        {
            CreateMap<CreateScreenGroupModel, ScreenGroup>().
                ForMember(dest => dest.ScreenGroupNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));
            CreateMap<UpdateScreenGroupModel, ScreenGroup>().
                ForMember(dest => dest.ScreenGroupNameTranslations, opt => opt.MapFrom(src => src.NameTranslations));


            CreateMap<NameTranslationModel, ScreenGroupNameTranslation>();
        }
    }
}