using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Core.Groups;

namespace Dawem.Models.AutoMapper.Core
{
    public class GroupMapProfile : Profile
    {
        public GroupMapProfile()
        {
            CreateMap<CreateGroupDTO, Group>();
            CreateMap<UpdateGroupDTO, Group>();
        }
    }
}
