using AutoMapper;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Attendances.Schedules;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper.Attendances.Schedules
{
    public class SchedulePlansMapProfile : Profile
    {
        public SchedulePlansMapProfile()
        {
            CreateMap<CreateSchedulePlanModel, SchedulePlan>()
                .ForMember(dest => dest.SchedulePlanEmployee, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.SchedulePlanGroup, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.SchedulePlanDepartment, opt => opt.MapFrom(src => src));

            CreateMap<CreateSchedulePlanModel, SchedulePlanEmployee>();
            CreateMap<CreateSchedulePlanModel, SchedulePlanGroup>();
            CreateMap<CreateSchedulePlanModel, SchedulePlanDepartment>();

            CreateMap<UpdateScheduleModel, SchedulePlan>()
                .ForMember(dest => dest.SchedulePlanEmployee, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.SchedulePlanGroup, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.SchedulePlanDepartment, opt => opt.MapFrom(src => src));

            CreateMap<UpdateScheduleModel, SchedulePlanEmployee>();
            CreateMap<UpdateScheduleModel, SchedulePlanGroup>();
            CreateMap<UpdateScheduleModel, SchedulePlanDepartment>();
        }
    }
}
