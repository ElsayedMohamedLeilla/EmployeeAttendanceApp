using Dawem.Models.Dtos.Providers;

namespace Dawem.Models.Response.Provider
{
    public class GetBranchesResponse : BaseResponse
    {
        public List<BranchDTO?>? Branches { get; set; }
    }
}
