using AutoMapper;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Models.AutoMapper.Dawem
{
    public class EmployeeOTPsMapProfile : Profile
    {
        public EmployeeOTPsMapProfile()
        {
            CreateMap<CreateEmployeeOTPDTO, EmployeeOTP>();
        }

    }
}
