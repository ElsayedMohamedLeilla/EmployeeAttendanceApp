using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Core.Groups;
using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupManagarDelegators;

namespace Dawem.Models.AutoMapper.Dawem.Core
{
    public class GroupMapProfile : Profile
    {
        public GroupMapProfile()
        {
            CreateMap<CreateGroupDTO, Group>()
                .ForMember(dest => dest.GroupEmployees, opt => opt.MapFrom(src => src.Employees))
                .ForMember(dest => dest.GroupManagerDelegators, opt => opt.MapFrom(src => src.ManagerDelegators))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));
            CreateMap<UpdateGroupDTO, Group>()
                .ForMember(dest => dest.GroupEmployees, opt => opt.MapFrom(src => src.Employees))
                .ForMember(dest => dest.GroupManagerDelegators, opt => opt.MapFrom(src => src.ManagerDelegators))
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));


            CreateMap<GroupEmployeeCreateModelDTO, GroupEmployee>();
            CreateMap<GroupManagarDelegatorCreateModelDTO, GroupManagerDelegator>();
            CreateMap<ZoneGroupCreateModelDTO, ZoneGroup>();



        }
    }
}
