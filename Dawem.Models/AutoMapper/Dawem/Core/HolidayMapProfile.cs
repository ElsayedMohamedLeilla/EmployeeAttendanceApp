using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.Holidays;

namespace Dawem.Models.AutoMapper.Dawem.Core
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
