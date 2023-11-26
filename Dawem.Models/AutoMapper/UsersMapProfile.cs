using AutoMapper;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Employees.User;

namespace Dawem.Models.AutoMapper
{
    public class UsersMapProfile : Profile
    {
        public UsersMapProfile()
        {
            CreateMap<CreateUserModel, MyUser>();
            CreateMap<UpdateUserModel, MyUser>();
            CreateMap<UserSignUpModel, MyUser>();

        }
    }
}
