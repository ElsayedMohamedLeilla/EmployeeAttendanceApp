using AutoMapper;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.ShiftWorkingTimes;

namespace Dawem.Models.AutoMapper.Schedules.Schedules
{
    public class ShiftWorkingTimeMapProfile : Profile
    {
        public ShiftWorkingTimeMapProfile()
        {
            CreateMap<CreateShiftWorkingTimeModelDTO, ShiftWorkingTime>();
            CreateMap<UpdateShiftWorkingTimeModelDTO, ShiftWorkingTime>();
        }
    }
}
