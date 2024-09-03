using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultTasksTypeMapProfile : Profile
    {
        public DefaultTasksTypeMapProfile()
        {
            CreateMap<CreateDefaultTasksTypeDTO, DefaultLookup>();
            CreateMap<UpdateDefaultTasksTypeDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
