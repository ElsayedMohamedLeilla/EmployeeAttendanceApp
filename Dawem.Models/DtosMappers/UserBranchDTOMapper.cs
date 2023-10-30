using Dawem.Domain.Entities.Provider;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.DtosMappers
{
    public class UserBranchDTOMapper
    {
        private static RequestInfo userContext;

        public static void InitUserContext(RequestInfo _userContext)
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
                UserGlobalName = userBranch.User?.Name ,
                BranchGlobalName = userContext.Lang == "ar" ? userBranch.Branch?.Name : ""


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
