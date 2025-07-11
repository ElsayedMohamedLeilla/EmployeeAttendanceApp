﻿using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Departments;

namespace Dawem.Models.AutoMapper.Dawem
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
            CreateMap<DepartmentZonesCreateModelDTO, ZoneDepartment>();

        }
    }
}
