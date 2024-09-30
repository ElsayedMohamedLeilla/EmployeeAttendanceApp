using AutoMapper;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.AutoMapper.AdminPanel.DefaultLookups
{
    public class DefaultDepartmentsMapProfile : Profile
    {
        public DefaultDepartmentsMapProfile()
        {
            CreateMap<CreateDefaultDepartmentsDTO, DefaultLookup>();
            CreateMap<UpdateDefaultDepartmentsDTO, DefaultLookup>();
            CreateMap<NameTranslationModel, DefaultLookupsNameTranslation>();

        }
    }
}
