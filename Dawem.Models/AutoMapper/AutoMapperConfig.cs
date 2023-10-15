using AutoMapper;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Dtos.Lookups;

namespace Dawem.Models.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Screen, ScreenDto>();


            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<User, UpdatedUser>().ForMember(dest => dest.UserRols, opt => opt.Ignore()).ForMember(dest => dest.userBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore());
            CreateMap<UpdatedUser, User>().ForMember(dest => dest.UserBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore());
            CreateMap<User, CreatedUser>().ForMember(dest => dest.UserRols, opt => opt.Ignore()).ForMember(dest => dest.UserBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore());
            CreateMap<CreatedUser, User>().ForMember(dest => dest.UserBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore()).ForMember(dest => dest.UserRols, opt => opt.Ignore());

            CreateMap<CreatedScreen, User>().ForMember(dest => dest.UserBranches, opt => opt.Ignore());
            CreateMap<CreatedScreen, Screen>().ReverseMap();




        }
    }
}
