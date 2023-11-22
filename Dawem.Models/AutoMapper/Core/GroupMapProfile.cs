using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Core.Group;
using Dawem.Models.Dtos.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Employees.Employees.GroupManagarDelegators;

namespace Dawem.Models.AutoMapper.Core
{
    public class GroupMapProfile : Profile
    {
        public GroupMapProfile()
        {
            CreateMap<CreateGroupDTO, Group>()
                .ForMember(dest => dest.GroupEmployees, opt => opt.MapFrom(src => src.Employees))
                .ForMember(dest => dest.GroupManagerDelegators, opt => opt.MapFrom(src => src.ManagerDelegators));
            CreateMap<UpdateGroupDTO, Group>()
                .ForMember(dest => dest.GroupEmployees, opt => opt.MapFrom(src => src.Employees))
                .ForMember(dest => dest.GroupManagerDelegators, opt => opt.MapFrom(src => src.ManagerDelegators));


            CreateMap<GroupEmployeeCreateModelDTO, GroupEmployee>();
            CreateMap<GroupManagarDelegatorCreateModelDTO, GroupManagerDelegator>();


        }
    }
}
