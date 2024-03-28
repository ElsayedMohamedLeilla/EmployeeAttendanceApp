using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;

namespace Dawem.Models.AutoMapper.Dawem.Core
{
    public class ResponsibilityMapProfile : Profile
    {
        public ResponsibilityMapProfile()
        {
            CreateMap<CreateResponsibilityModel, Responsibility>();
            CreateMap<UpdateResponsibilityModel, Responsibility>();
        }
    }
}
