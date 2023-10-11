using SmartBusinessERP.Models.Dtos.Provider;

namespace SmartBusinessERP.Models.Response.Others
{
    public class GetUserBranchesResponse : BaseResponse
    {
        public List<BranchLiteDTO>? UserBranches { get; set; }
    }
}
