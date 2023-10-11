using SmartBusinessERP.Models.Dtos.Provider;

namespace SmartBusinessERP.Models.Response.Core
{
    public class GetBranchInfoResponse : BaseResponse
    {
        public BranchDTO? BranchInfo { get; set; }
    }
}
