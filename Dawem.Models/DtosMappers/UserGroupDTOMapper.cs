using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Core;

namespace Dawem.Models.DtosMappers
{
    public class UserGroupDTOMapper
    {
        private static RequestInfo userContext;

        public static void InitUserContext(RequestInfo _userContext)
        {
            userContext = _userContext;
        }


        public static UserGroupDTO? Map(UserGroup userGroup)
        {
            if (userGroup == null) return null;
            var DTO = new UserGroupDTO()
            {
                Id = userGroup.Id,
                GroupId = userGroup.GroupId,
                UserId = userGroup.UserId,
                UserGlobalName = userGroup.User?.Name,
                GroupGlobalName = userContext.Lang == "ar" ? userGroup.Group?.NameAr : userGroup.Group?.NameEn


            };
            return DTO;
        }



        public static List<UserGroupDTO?>? Map(List<UserGroup?>? userGroupes)
        {
            if (userGroupes == null) return null;
            return userGroupes.Select(selector: Map).ToList();
        }



    }
}
