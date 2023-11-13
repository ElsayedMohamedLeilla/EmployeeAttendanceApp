using AutoMapper;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper.Attendances.Schedules
{
    public class SchedulePlansMapProfile : Profile
    {
        public SchedulePlansMapProfile()
        {
            CreateMap<CreateSchedulePlanModel, SchedulePlan>()
                 .AfterMap(MapRelated);

            CreateMap<CreateSchedulePlanModel, SchedulePlanEmployee>();
            CreateMap<CreateSchedulePlanModel, SchedulePlanGroup>();
            CreateMap<CreateSchedulePlanModel, SchedulePlanDepartment>();

            CreateMap<UpdateSchedulePlanModel, SchedulePlan>()
                .AfterMap(MapRelated);


            CreateMap<UpdateSchedulePlanModel, SchedulePlanEmployee>();
            CreateMap<UpdateSchedulePlanModel, SchedulePlanGroup>();
            CreateMap<UpdateSchedulePlanModel, SchedulePlanDepartment>();
        }
        public void MapRelated(CreateSchedulePlanModel src, SchedulePlan dest, ResolutionContext context)
        {
            dest.SchedulePlanEmployee = src.EmployeeId > 0 ? context.Mapper.Map<SchedulePlanEmployee>(src) : null;
            dest.SchedulePlanGroup = src.GroupId > 0 ? context.Mapper.Map<SchedulePlanGroup>(src) : null;
            dest.SchedulePlanDepartment = src.DepartmentId > 0 ? context.Mapper.Map<SchedulePlanDepartment>(src) : null;
        }
        public void MapRelated(UpdateSchedulePlanModel src, SchedulePlan dest, ResolutionContext context)
        {
            dest.SchedulePlanEmployee = src.EmployeeId > 0 ? context.Mapper.Map<SchedulePlanEmployee>(src) : null;
            dest.SchedulePlanGroup = src.GroupId > 0 ? context.Mapper.Map<SchedulePlanGroup>(src) : null;
            dest.SchedulePlanDepartment = src.DepartmentId > 0 ? context.Mapper.Map<SchedulePlanDepartment>(src) : null;
        }
    }
}
