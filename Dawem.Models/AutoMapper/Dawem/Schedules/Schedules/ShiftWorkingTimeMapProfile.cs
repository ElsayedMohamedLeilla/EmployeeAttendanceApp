using AutoMapper;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes;

namespace Dawem.Models.AutoMapper.Dawem.Schedules.Schedules
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
