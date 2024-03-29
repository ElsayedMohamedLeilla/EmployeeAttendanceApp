using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;

namespace Dawem.Models.AutoMapper.Dawem.Core
{
    public class VacationsTypeMapProfile : Profile
    {
        public VacationsTypeMapProfile()
        {
            CreateMap<CreateVacationsTypeDTO, VacationType>();
            CreateMap<UpdateVacationsTypeDTO, VacationType>();
        }
    }
}
