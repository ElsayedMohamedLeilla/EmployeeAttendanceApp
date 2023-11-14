using AutoMapper;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper.Attendances.WeekAttendances
{
    public class SchedulesMapProfile : Profile
    {
        public SchedulesMapProfile()
        {
            CreateMap<CreateScheduleModel, Schedule>().ForMember(dest => dest.ScheduleDays, opt => opt.MapFrom(src => src.ScheduleDays));
            CreateMap<UpdateScheduleModel, Schedule>().ForMember(dest => dest.ScheduleDays, opt => opt.MapFrom(src => src.ScheduleDays));

            CreateMap<ScheduleDayCreateModel, ScheduleDay>();
        }
    }
}
