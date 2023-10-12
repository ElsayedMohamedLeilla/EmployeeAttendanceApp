using Dawem.Enums.General;
using Dawem.Models.Context;
using Dawem.Models.Response;
using Dawem.Models.Response.Provider;
using SmartBusinessERP.Models.Criteria.Provider;

namespace SmartBusinessERP.BusinessLogic.Validators.Contract
{
    public interface IBranchValidatorBL
    {
        Task<BranchValidatorResult> BranchCreationValidator(BranchValidatorModel branchValidatorModel);
        BaseResponseT<bool> ValidateChangeForMainBranchOnly(RequestHeaderContext userContext, ChangeType changeType);
        Task<validateUserBranchResult> ValidateUserBranch(ValidateUserBranchSearchCriteria validateUserBranchSearchCriteria);
    }
}
