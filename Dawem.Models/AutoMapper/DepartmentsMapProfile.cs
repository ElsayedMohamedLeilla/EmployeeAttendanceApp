using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Department;

namespace Dawem.Models.AutoMapper
{
    public class DepartmentsMapProfile : Profile
    {
        public DepartmentsMapProfile()
        {
            CreateMap<CreateDepartmentModel, Department>()
                .ForMember(dest => dest.ManagerDelegators, opt => opt.MapFrom(src => src.ManagerDelegators))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));
            CreateMap<UpdateDepartmentModel, Department>()
                .ForMember(dest => dest.ManagerDelegators, opt => opt.MapFrom(src => src.ManagerDelegators))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));


            CreateMap<DepartmentManagarDelegatorCreateModelDTO, DepartmentManagerDelegator>();
            CreateMap<DepartmentZonesCreateModelDTO, DepartmentZone>();

        }
    }
}
