using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others;

namespace Dawem.Models.DtosMappers.Others
{
    public class UserScreenActionPermissionDTOMapper
    {
        private static RequestInfo? userContext;

        public static void InitUserScreenActionPermissionContext(RequestInfo _userContext)
        {
            userContext = _userContext;
        }
        public static UserScreenActionPermissionDTO? Map(Permission? userScreenActionPermission)
        {
            if (userScreenActionPermission == null) return null;
            var DTO = new UserScreenActionPermissionDTO()
            {
                Id = userScreenActionPermission.Id,
                AddedDate = userScreenActionPermission.AddedDate/*,
                ActionType = userScreenActionPermission.ActionType,
                ActionPlace = userScreenActionPermission.ScreenCode,
                UserId = userScreenActionPermission.UserId,
                GroupId = userScreenActionPermission.GroupId*/
            };
            return DTO;
        }


        public static List<UserScreenActionPermissionDTO?>? Map(List<Permission?>? userScreenActionPermissions)
        {
            if (userScreenActionPermissions == null) return null;
            return userScreenActionPermissions.Select(selector: Map).ToList();
        }

    }
}
