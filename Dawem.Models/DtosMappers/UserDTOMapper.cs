using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Identity;

namespace SmartBusinessERP.Models.DtosMappers
{
    public class UserDTOMapper
    {

        private static RequestHeaderContext userContext;

        public static void InitUserContext(RequestHeaderContext _userContext)
        {
            userContext = _userContext;
            UserGroupDTOMapper.InitUserContext(userContext);
            UserBranchDTOMapper.InitUserContext(userContext);
        }

        public static UserDTO Map(User user)
        {
            if (user == null) return null;
            var DTO = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,

                MobileNumber = user.MobileNumber,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                AddedDate = user.AddedDate
            };
            return DTO;
        }
        public static List<UserDTO> MapListUsers(List<User> users)
        {
            if (users == null) return null;

            return users.Select(Map).ToList();
        }

        public static SmartUserInfo MapInfo(User user)
        {
            if (user == null) return null;
            var DTO = new SmartUserInfo()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobileNumber = user.MobileNumber,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                AddedDate = user.AddedDate,
                BranchId = user.MainBranchId ?? 0,
                AddUserId = user.AddUserId,
                UserBranches = UserBranchDTOMapper.Map(user.UserBranches),
                UserGroups = UserGroupDTOMapper.Map(user.UserGroups)

            };
            return DTO;
        }



        public static User Map(UserDTO userDTO)
        {
            if (userDTO == null) return null;
            var _user = new User()
            {
                Id = userDTO.Id,
                Email = userDTO.Email,
                UserName = userDTO.UserName,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                BirthDate = userDTO.BirthDate,
                PhoneNumber = userDTO.PhoneNumber,
                MobileNumber = userDTO.MobileNumber,
                IsActive = userDTO.IsActive,
            };
            return _user;
        }

    }
}
