using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.AutoMapper
{
    public class EmployeesMapProfile : Profile
    {
        public EmployeesMapProfile()
        {
            CreateMap<CreateEmployeeModel, Employee>();
            CreateMap<UpdateEmployeeModel, Employee>();
        }
    }
}
