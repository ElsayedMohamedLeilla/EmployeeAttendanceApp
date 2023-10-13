using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.ResponseModels
{
    public class GetBranchesResponseModel
    {
        public List<BranchDTO> Branches { get; set; }
        public int TotalCount { get; set; }
    }
}
