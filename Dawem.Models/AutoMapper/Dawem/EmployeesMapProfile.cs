using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Models.AutoMapper.Dawem
{
    public class EmployeesMapProfile : Profile
    {
        public EmployeesMapProfile()
        {
            CreateMap<CreateEmployeeModel, Employee>()
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));

            CreateMap<UpdateEmployeeModel, Employee>()
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));
            CreateMap<UserSignUpModel, Employee>();

            CreateMap<EmployeeZonesCreateModelDTO, ZoneEmployee>();
            CreateMap<EmployeeZonesUpdateModelDTO, ZoneEmployee>();




        }
    }
}
