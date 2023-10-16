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


            CreateMap<MyUser, UserDTO>().ReverseMap();

            CreateMap<MyUser, UpdatedUser>().ForMember(dest => dest.UserRols, opt => opt.Ignore()).ForMember(dest => dest.userBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore());
            CreateMap<UpdatedUser, MyUser>().ForMember(dest => dest.UserBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore());
            CreateMap<MyUser, CreatedUser>().ForMember(dest => dest.UserRols, opt => opt.Ignore()).ForMember(dest => dest.UserBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore());
            CreateMap<CreatedUser, MyUser>().ForMember(dest => dest.UserBranches, opt => opt.Ignore()).ForMember(dest => dest.UserGroups, opt => opt.Ignore()).ForMember(dest => dest.UserRols, opt => opt.Ignore());

            CreateMap<CreatedScreen, MyUser>().ForMember(dest => dest.UserBranches, opt => opt.Ignore());
            CreateMap<CreatedScreen, Screen>().ReverseMap();




        }
    }
}
