using AutoMapper;
using SmartBusinessERP.Areas.Identity.Data.UserManagement;
using SmartBusinessERP.Domain.Entities.Core;
using SmartBusinessERP.Domain.Entities.Lookups;
using SmartBusinessERP.Domain.Entities.Packages;
using SmartBusinessERP.Domain.Entities.Provider;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Dtos.Identity;
using SmartBusinessERP.Models.Dtos.Lookups;
using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Domain.Entities.Accounts;
using SmartBusinessERP.Models.Dtos.Accounting;
using SmartBusinessERP.Domain.Entities.Inventory;
using Dawem.Models.Dtos.Lookups;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Identity;

namespace SmartBusinessERP.BusinessLogic
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
