using AutoMapper;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper.Attendances.WeekAttendances
{
    public class WeekAttendancesMapProfile : Profile
    {
        public WeekAttendancesMapProfile()
        {
            CreateMap<CreateWeekAttendanceModel, WeekAttendance>()
                .ForMember(dest => dest.WeekAttendanceShifts, opt => opt.MapFrom(src => src.WeekShifts));
            CreateMap<UpdateWeekAttendanceModel, WeekAttendance>()
                 .ForMember(dest => dest.WeekAttendanceShifts, opt => opt.MapFrom(src => src.WeekShifts));

            CreateMap<WeekAttendanceShiftCreateModel, WeekAttendanceShift>();
        }
    }
}
