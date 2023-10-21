using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Provider;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Provider;
using Dawem.Models.DtosMappers;
using Dawem.Models.Exceptions;
using Dawem.Models.ResponseModels;
using Dawem.Models.Validation;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Provider
{
    public class BranchBL : IBranchBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IBranchRepository branchRepository;
        private readonly RequestInfo requestHeaderContext;
        private readonly IBranchBLValidation branchBLValidation;
        private readonly IUserBranchBL userBranchBL;

        public BranchBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
               RequestInfo _userContext, IBranchRepository _branchRepository,
               IBranchBLValidation _BranchValidatorBL, IUserBranchBL _userBranchBL)
        {
            unitOfWork = _unitOfWork;
            requestHeaderContext = _userContext;
            branchBLValidation = _BranchValidatorBL;
            branchRepository = _branchRepository;
            userBranchBL = _userBranchBL;
        }

        public async Task<int> Create(BranchDTO branchDTO)
        {
            #region Validation

            branchBLValidation.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Add);

            var BranchValidatorModel = new BranchValidatorModel
            {
                Branch = branchDTO,
                ChangeType = ChangeType.Add
            };

            branchBLValidation.BranchCreationValidator(BranchValidatorModel);

            #endregion

            if (branchDTO.CompanyId <= 0)
            {
                branchDTO.CompanyId = requestHeaderContext.CompanyId;
            }

            var branch = BranchDTOMapper.Map(branchDTO);
            branchRepository.Insert(branch);
            await unitOfWork.SaveAsync();

            return branch.Id;
        }

        public async Task<BranchDTO> GetInfo(GetBranchInfoCriteria criteria)
        {
            var branch = await branchRepository.
                GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, nameof(Branch.Country)) ??
                throw new BusinessValidationException(DawemKeys.BranchNotFound);

            BranchDTOMapper.InitBranchContext(requestHeaderContext);
            var branchInfo = BranchDTOMapper.Map(branch);

            return branchInfo;
        }
        public async Task<GetBranchesResponseModel> Get(GetBranchesCriteria criteria)
        {
            var query = branchRepository.GetAsQueryable(criteria, nameof(Branch.Country));

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = branchRepository.OrderBy(query, "Id", "desc");

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var branchesList = await queryPaged.ToListAsync();

            BranchDTOMapper.InitBranchContext(requestHeaderContext);
            var branches = BranchDTOMapper.Map(branchesList);

            return new GetBranchesResponseModel
            {
                Branches = branches,
                TotalCount = await query.CountAsync()
            };
        }
        public async Task<bool> Update(BranchDTO branchDTO)
        {
            branchBLValidation.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Edit);

            branchDTO.CompanyId = requestHeaderContext.CompanyId;
            var branch = BranchDTOMapper.Map(branchDTO);
            branchRepository.Update(branch);
            await unitOfWork.SaveAsync();

            return true;
        }
        public async Task<bool> Delete(int Id)
        {
            branchBLValidation.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Delete);
            
            branchRepository.Delete(Id);
            await unitOfWork.SaveAsync();

            return true;
        }
    }
}
