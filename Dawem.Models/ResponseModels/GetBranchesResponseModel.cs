using Dawem.Models.Dtos.Providers;

namespace Dawem.Models.ResponseModels
{
    public class GetBranchesResponseModel
    {
        public List<BranchDTO> Branches { get; set; }
        public int TotalCount { get; set; }
    }
}
