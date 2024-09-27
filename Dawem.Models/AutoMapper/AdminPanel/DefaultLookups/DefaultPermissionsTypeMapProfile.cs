using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultPermissionsTypeMapProfile : Profile
    {
        public DefaultPermissionsTypeMapProfile()
        {
            CreateMap<CreateDefaultPermissionsTypeDTO, DefaultLookup>();
            CreateMap<UpdateDefaultPermissionsTypeDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
