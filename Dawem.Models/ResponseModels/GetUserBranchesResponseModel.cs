using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.ResponseModels
{
    public class GetUserBranchesResponseModel
    {
        public List<BranchLiteDTO> UserBranches { get; set; }
        public int TotalCount { get; set; }
    }
}
