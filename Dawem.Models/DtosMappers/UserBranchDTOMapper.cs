using Dawem.Domain.Entities.Provider;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Provider;

namespace SmartBusinessERP.Models.DtosMappers
{
    public class UserBranchDTOMapper
    {
        private static RequestHeaderContext userContext;

        public static void InitUserContext(RequestHeaderContext _userContext)
        {
            userContext = _userContext;
        }


        public static UserBranchDTO? Map(UserBranch userBranch)
        {
            if (userBranch == null) return null;
            var DTO = new UserBranchDTO()
            {
                Id = userBranch.Id,
                BranchId = userBranch.BranchId,
                UserId = userBranch.UserId,
                UserGlobalName = userBranch.User?.FirstName + " " + userBranch.User?.LastName,
                BranchGlobalName = userContext.Lang == "ar" ? userBranch.Branch?.BranchName : ""


            };
            return DTO;
        }



        public static List<UserBranchDTO?>? Map(List<UserBranch?>? userBranches)
        {
            if (userBranches == null) return null;
            return userBranches.Select(selector: Map).ToList();
        }



    }
}
