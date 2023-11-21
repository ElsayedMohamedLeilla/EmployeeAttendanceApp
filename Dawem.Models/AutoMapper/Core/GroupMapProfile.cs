using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Core.Groups;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper.Core
{
    public class GroupMapProfile : Profile
    {
        public GroupMapProfile()
        {
            CreateMap<CreateGroupDTO, Group>()
                .ForMember(dest => dest.GroupEmployees, opt => opt.MapFrom(src => src.GroupEmployees))
                .ForMember(dest => dest.GroupManagerDelegators, opt => opt.MapFrom(src => src.GroupManagerDelegators));
            CreateMap<UpdateGroupDTO, Group>()
                .ForMember(dest => dest.GroupEmployees, opt => opt.MapFrom(src => src.GroupEmployees))
                .ForMember(dest => dest.GroupManagerDelegators, opt => opt.MapFrom(src => src.GroupManagerDelegators));


            CreateMap<GroupEmployeeCreateModelDTO, GroupEmployee>();
            CreateMap<GroupManagarDelegatorCreateModelDTO, GroupManagerDelegator>();


        }
    }
}
