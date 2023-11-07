using AutoMapper;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper.Core
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
