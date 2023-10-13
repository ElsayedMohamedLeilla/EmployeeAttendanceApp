using Dawem.Enums.General;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Response;
using Dawem.Models.Response.Provider;
using Dawem.Models.Validation;

namespace Dawem.Contract.BusinessValidation
{
    public interface IBranchBLValidation
    {
        Task<BranchValidatorResult> BranchCreationValidator(BranchValidatorModel branchValidatorModel);
        BaseResponseT<bool> ValidateChangeForMainBranchOnly(RequestHeaderContext userContext, ChangeType changeType);
        Task<validateUserBranchResult> ValidateUserBranch(ValidateUserBranchSearchCriteria validateUserBranchSearchCriteria);
    }
}
