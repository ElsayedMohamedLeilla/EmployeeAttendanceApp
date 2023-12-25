using Dawem.Models.Dtos.Providers;

namespace Dawem.Models.Response.Others
{
    public class GetUserBranchesResponse : BaseResponse
    {
        public List<BranchLiteDTO>? UserBranches { get; set; }
    }
}
