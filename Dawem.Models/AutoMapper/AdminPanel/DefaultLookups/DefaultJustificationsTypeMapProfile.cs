using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultJustificationsTypeMapProfile : Profile
    {
        public DefaultJustificationsTypeMapProfile()
        {
            CreateMap<CreateDefaultJustificationsTypeDTO, DefaultLookup>();
            CreateMap<UpdateDefaultJustificationsTypeDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
