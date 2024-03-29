using AutoMapper;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Models.AutoMapper.Dawem
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
