using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultShiftsTypeMapProfile : Profile
    {
        public DefaultShiftsTypeMapProfile()
        {
            CreateMap<CreateDefaultShiftsTypeDTO, DefaultLookup>();
            CreateMap<UpdateDefaultShiftsTypeDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
