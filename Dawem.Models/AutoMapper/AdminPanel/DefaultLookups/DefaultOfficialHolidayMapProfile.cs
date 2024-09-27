using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultOfficialHolidaysTypeMapProfile : Profile
    {
        public DefaultOfficialHolidaysTypeMapProfile()
        {
            CreateMap<CreateDefaultOfficialHolidaysDTO, DefaultLookup>();
            CreateMap<UpdateDefaultOfficialHolidaysDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
