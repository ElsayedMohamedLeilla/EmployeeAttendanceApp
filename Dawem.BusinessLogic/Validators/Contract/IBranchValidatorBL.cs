using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Criteria.Provider;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Provider;

namespace SmartBusinessERP.BusinessLogic.Validators.Contract
{
    public interface IBranchValidatorBL
    {
        Task<BranchValidatorResult> BranchCreationValidator(BranchValidatorModel branchValidatorModel);
        BaseResponseT<bool> ValidateChangeForMainBranchOnly(RequestHeaderContext userContext, ChangeType changeType);
        Task<validateUserBranchResult> ValidateUserBranch(ValidateUserBranchSearchCriteria validateUserBranchSearchCriteria);
    }
}
