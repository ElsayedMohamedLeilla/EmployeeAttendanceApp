using Dawem.Models.Dtos.Providers;

namespace Dawem.Models.Response.Core
{
    public class GetBranchInfoResponse : BaseResponse
    {
        public BranchDTO? BranchInfo { get; set; }
    }
}
