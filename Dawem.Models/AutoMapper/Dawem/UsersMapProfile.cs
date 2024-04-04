using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Models.AutoMapper.Dawem
{
    public class UsersMapProfile : Profile
    {
        public UsersMapProfile()
        {
            CreateMap<CreateUserModel, MyUser>().
                AfterMap(MapUserResponsibilities);
            CreateMap<UpdateUserModel, MyUser>().
                AfterMap(MapUserResponsibilities);

            CreateMap<UserSignUpModel, MyUser>();

            CreateMap<AdminPanelCreateUserModel, MyUser>().
                AfterMap(MapUserResponsibilities);
            CreateMap<AdminPanelUpdateUserModel, MyUser>().
                AfterMap(MapUserResponsibilities);

        }
        private void MapUserResponsibilities(CreateUserModel source, MyUser destination, ResolutionContext context)
        {
            destination.UserResponsibilities = source.Responsibilities
                .Select(responsibilityId => new UserResponsibility { ResponsibilityId = responsibilityId })
                .ToList();
        }
        private void MapUserResponsibilities(UpdateUserModel source, MyUser destination, ResolutionContext context)
        {
            destination.UserResponsibilities = source.Responsibilities
                .Select(responsibilityId => new UserResponsibility { ResponsibilityId = responsibilityId })
                .ToList();
        }
        private void MapUserResponsibilities(AdminPanelCreateUserModel source, MyUser destination, ResolutionContext context)
        {
            destination.UserResponsibilities = source.Responsibilities
                .Select(responsibilityId => new UserResponsibility { ResponsibilityId = responsibilityId })
                .ToList();
        }
        private void MapUserResponsibilities(AdminPanelUpdateUserModel source, MyUser destination, ResolutionContext context)
        {
            destination.UserResponsibilities = source.Responsibilities
                .Select(responsibilityId => new UserResponsibility { ResponsibilityId = responsibilityId })
                .ToList();
        }
    }
}
