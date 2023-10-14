using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.General;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Provider;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Dawem.Validation.BusinessValidation
{

    public class BranchBLValidation : IBranchBLValidation
    {
        private readonly IRepositoryManager repositoryManager;

        public BranchBLValidation(IRepositoryManager _repositoryManager)
        {
            repositoryManager = _repositoryManager;
        }
        public BranchDTO BranchCreationValidator(Models.Validation.BranchValidatorModel model)
        {
            var branch = model.Branch;


            if (branch.CountryId <= 0)
            {
                throw new BusinessValidationErrorException(DawemKeys.YouMustChooseCountry);
            }

            return branch;
        }
        public bool ValidateChangeForMainBranchOnly(RequestHeaderContext userContext, ChangeType changeType)
        {

            if (userContext == null)
            {
                throw new BusinessValidationErrorException(DawemKeys.SorryuserContextNull);
            }

            if (!userContext.IsMainBranch && changeType == ChangeType.View)
            {
                throw new BusinessValidationErrorException(DawemKeys.SorryYouCurrentBranchIsNotMainBranch);
            }

            if (!userContext.IsMainBranch && changeType == ChangeType.Add)
            {
                throw new BusinessValidationErrorException(DawemKeys.SorryAddCanBeDoneInMainBranchOnly);
            }

            if (!userContext.IsMainBranch && changeType == ChangeType.Edit)
            {
                throw new BusinessValidationErrorException(DawemKeys.SorryEditCanBeDoneInMainBranchOnly);
            }

            if (!userContext.IsMainBranch && changeType == ChangeType.Delete)
            {
                throw new BusinessValidationErrorException(DawemKeys.SorryDeleteCanBeDoneInMainBranchOnly);
            }
            return true;
        }

        public async Task<int> ValidateUserBranch(ValidateUserBranchSearchCriteria criteria)
        {
            var response = new validateUserBranchResult();
            response.Status = ResponseStatus.Success;

            var branchId = criteria.BranchId;


            if (branchId <= 0)
            {
                var uProviderQuery = repositoryManager.UserBranchRepository
                    .GetByCondition(ub => ub.UserId == criteria.UserId &&
                     ub.Branch != null && ub.Branch.IsMainBranch)
                    .Select(ua => ua.BranchId);

                var branchs = await uProviderQuery.ToListAsync();

                if (branchs.Count > 1)
                {
                    throw new BusinessValidationErrorException(DawemKeys.SorryThereIsMoreThanOneBranchForChoosenUserYouMustChooseBranchToEnterWith);
                }
                branchId = branchs.FirstOrDefault();
            }

            var getUserBranch = await repositoryManager.UserBranchRepository
                .GetEntityByConditionAsync(uas => uas.UserId == criteria.UserId && uas.BranchId == branchId);

            return getUserBranch == null
                ? throw new BusinessValidationErrorException(DawemKeys.SorryThisUserDoNotHaveAccessToSelectedBranch)
                : branchId;
        }
    }
}
