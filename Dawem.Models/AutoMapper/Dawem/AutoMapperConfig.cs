using AutoMapper;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Dawem.Identities;
using Dawem.Models.Dtos.Dawem.Lookups;

namespace Dawem.Models.AutoMapper.Dawem
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<OldNotUsedScreen, ScreenDto>();


            CreateMap<MyUser, UserDTO>().ReverseMap();

            CreateMap<MyUser, UpdatedUser>().ForMember(dest => dest.UserRols, opt => opt.Ignore()).ForMember(dest => dest.userBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore());
            CreateMap<CreatedScreen, MyUser>().ForMember(dest => dest.UserBranches, opt => opt.Ignore());
            CreateMap<CreatedScreen, OldNotUsedScreen>().ReverseMap();




        }
    }
}
