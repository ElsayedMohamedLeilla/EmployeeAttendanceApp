using SmartBusinessERP.Models.Dtos.Provider;

namespace SmartBusinessERP.Models.Response.Provider
{
    public class GetBranchesResponse : BaseResponse
    {
        public List<BranchDTO?>? Branches { get; set; }
    }
}
