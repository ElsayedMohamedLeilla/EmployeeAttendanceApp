using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Core.VacationsTypes;

namespace Dawem.Models.AutoMapper.Core
{
    public class VacationsTypeMapProfile : Profile
    {
        public VacationsTypeMapProfile()
        {
            CreateMap<CreateVacationsTypeDTO, VacationsType>();
            CreateMap<UpdateVacationsTypeDTO, VacationsType>();
        }
    }
}
