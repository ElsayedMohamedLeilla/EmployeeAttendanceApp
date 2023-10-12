using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Response.Core
{
    public class GetBranchInfoResponse : BaseResponse
    {
        public BranchDTO? BranchInfo { get; set; }
    }
}
