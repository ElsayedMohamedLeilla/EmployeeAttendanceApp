using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultPenaltiesMapProfile : Profile
    {
        public DefaultPenaltiesMapProfile()
        {
            CreateMap<CreateDefaultPenaltiesDTO, DefaultLookup>();
            CreateMap<UpdateDefaultPenaltiesDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
