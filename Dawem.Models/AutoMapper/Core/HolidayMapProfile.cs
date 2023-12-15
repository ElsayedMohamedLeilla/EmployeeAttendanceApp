using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Core.Holidaies;

namespace Dawem.Models.AutoMapper.Core
{
    public class HolidayMapProfile : Profile
    {
        public HolidayMapProfile()
        {
            CreateMap<CreateHolidayDTO, Holiday>();
            CreateMap<UpdateHolidayDTO, Holiday>();
        }
    }
}
