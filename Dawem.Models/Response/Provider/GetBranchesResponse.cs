using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Response.Provider
{
    public class GetBranchesResponse : BaseResponse
    {
        public List<BranchDTO?>? Branches { get; set; }
    }
}
