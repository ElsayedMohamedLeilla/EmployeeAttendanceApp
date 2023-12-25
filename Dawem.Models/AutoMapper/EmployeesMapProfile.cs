using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Employees.Users;

namespace Dawem.Models.AutoMapper
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
