using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultJobTitlesMapProfile : Profile
    {
        public DefaultJobTitlesMapProfile()
        {
            CreateMap<CreateDefaultJobTitlesDTO, DefaultLookup>();
            CreateMap<UpdateDefaultJobTitlesDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
