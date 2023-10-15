using Dawem.Enums.General;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Validation;

namespace Dawem.Contract.BusinessValidation
{
    public interface IBranchBLValidation
    {
        BranchDTO BranchCreationValidator(BranchValidatorModel branchValidatorModel);
        bool ValidateChangeForMainBranchOnly(RequestHeaderContext requestHeaderContext, ChangeType changeType);
        Task<int> ValidateUserBranch(ValidateUserBranchSearchCriteria validateUserBranchSearchCriteria);
    }
}
