using Dawem.Models.Dtos.Dawem.Providers;

namespace Dawem.Models.Response.Dawem.Others
{
    public class GetUserBranchesResponse : BaseResponse
    {
        public List<BranchLiteDTO> UserBranches { get; set; }
    }
}
