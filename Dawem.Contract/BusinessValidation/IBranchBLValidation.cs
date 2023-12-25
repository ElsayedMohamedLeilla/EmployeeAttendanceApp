using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Providers;
using Dawem.Models.Validations;

namespace Dawem.Contract.BusinessValidation
{
    public interface IBranchBLValidation
    {
        BranchDTO BranchCreationValidator(BranchValidatorModel branchValidatorModel);
        bool ValidateChangeForMainBranchOnly(RequestInfo requestHeaderContext, ChangeType changeType);
        Task<int> ValidateUserBranch(ValidateUserBranchSearchCriteria validateUserBranchSearchCriteria);
    }
}
