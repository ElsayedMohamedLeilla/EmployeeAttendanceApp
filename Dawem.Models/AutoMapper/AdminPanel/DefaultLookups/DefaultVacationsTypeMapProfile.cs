using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.Dawem.Core
{
    public class DefaultVacationsTypeMapProfile : Profile
    {
        public DefaultVacationsTypeMapProfile()
        {
            CreateMap<CreateDefaultVacationsTypeDTO, DefaultLookup>();
            CreateMap<UpdateDefaultVacationsTypeDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
