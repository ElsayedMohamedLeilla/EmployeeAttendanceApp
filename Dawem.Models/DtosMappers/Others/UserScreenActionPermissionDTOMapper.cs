using Dawem.Domain.Entities.Others;
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
        public static UserScreenActionPermissionDTO? Map(UserScreenActionPermission? userScreenActionPermission)
        {
            if (userScreenActionPermission == null) return null;
            var DTO = new UserScreenActionPermissionDTO()
            {
                Id = userScreenActionPermission.Id,
                AddedDate = userScreenActionPermission.AddedDate,
                ActionType = userScreenActionPermission.ActionType,
                ActionPlace = userScreenActionPermission.ActionPlace,
                UserId = userScreenActionPermission.UserId,
                GroupId = userScreenActionPermission.GroupId
            };
            return DTO;
        }


        public static List<UserScreenActionPermissionDTO?>? Map(List<UserScreenActionPermission?>? userScreenActionPermissions)
        {
            if (userScreenActionPermissions == null) return null;
            return userScreenActionPermissions.Select(selector: Map).ToList();
        }

    }
}
