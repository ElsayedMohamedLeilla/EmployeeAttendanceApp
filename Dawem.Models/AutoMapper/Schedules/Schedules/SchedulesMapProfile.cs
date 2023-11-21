using AutoMapper;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Schedules.Schedules;

namespace Dawem.Models.AutoMapper.Schedules.Schedules
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
