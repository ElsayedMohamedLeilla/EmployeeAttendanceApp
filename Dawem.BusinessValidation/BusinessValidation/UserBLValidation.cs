using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Provider;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Exceptions;
using Dawem.Translations;

namespace Dawem.Validation.BusinessValidation
{

    public class UserBLValidation : IUserBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        public UserBLValidation(IRepositoryManager _repositoryManager)
        {
            repositoryManager = _repositoryManager;
        }

        public async Task<bool> CreateUserValidation(CreatedUser createdUser)
        {
            if (createdUser == null)
            {
                throw new BusinessValidationException(DawemKeys.SorryYouMustEnterUser);
            }

            #region Validate Email

            var checkEmailDuplicate= await repositoryManager.UserRepository
                .GetEntityByConditionAsync(u=> u.Id != createdUser.Id && u.Email == createdUser.Email.Trim());

            if (createdUser is not null)
            {
                throw new BusinessValidationException(DawemKeys.SorryUserEmailIsDuplicatedYouMustEnterUniqueEmail);
            }

            #endregion

            if (createdUser.UserBranches == null ||
                   createdUser.UserBranches.Count <= 0)
            {
                if (createdUser.MainBranchId <= 0)
                {
                    throw new BusinessValidationException(DawemKeys.SorryYouMustChooseOneOrMoreBranchForTheUser);
                }
                else
                {
                    createdUser.UserBranches = new List<UserBranch>
                    {
                        new UserBranch()
                        {
                            BranchId = createdUser.MainBranchId
                        }
                    };
                }
            }

            return true;
        }
    }
}
