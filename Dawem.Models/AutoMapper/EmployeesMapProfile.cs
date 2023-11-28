using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Core.Group;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Employees.User;

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
