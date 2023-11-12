using AutoMapper;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Attendances.ShiftWorkingTimes;

namespace Dawem.Models.AutoMapper.Attendances.Schedules
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
