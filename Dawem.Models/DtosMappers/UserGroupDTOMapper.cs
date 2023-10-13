using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Core;

namespace Dawem.Models.DtosMappers
{
    public class UserGroupDTOMapper
    {
        private static RequestHeaderContext userContext;

        public static void InitUserContext(RequestHeaderContext _userContext)
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
                UserGlobalName = userGroup.User?.FirstName + " " + userGroup.User?.LastName,
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
