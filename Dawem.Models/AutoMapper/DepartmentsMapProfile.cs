using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Department;

namespace Dawem.Models.AutoMapper
{
    public class DepartmentsMapProfile : Profile
    {
        public DepartmentsMapProfile()
        {
            CreateMap<CreateDepartmentModel, Department>();
            CreateMap<UpdateDepartmentModel, Department>();
        }
    }
}
