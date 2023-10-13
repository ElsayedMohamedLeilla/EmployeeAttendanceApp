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
using Dawem.Translations;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dawem.BusinessLogic.Provider
{
    public class BranchBL : IBranchBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IBranchRepository branchRepository;
        private readonly RequestHeaderContext requestHeaderContext;
        private readonly IBranchBLValidation BranchValidatorBL;
        private readonly IBranchBLValidation mainBranchValidatorBL;
        private readonly IUserBranchBL userBranchBL;

        public BranchBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IBranchBLValidation _mainBranchValidatorBL,
               RequestHeaderContext _userContext, IBranchRepository _branchRepository,
               IBranchBLValidation _BranchValidatorBL, IUserBranchBL _userBranchBL)
        {
            unitOfWork = _unitOfWork;
            requestHeaderContext = _userContext;
            mainBranchValidatorBL = _mainBranchValidatorBL;
            BranchValidatorBL = _BranchValidatorBL;
            branchRepository = _branchRepository;
            userBranchBL = _userBranchBL;
        }

        public async Task<int> Create(BranchDTO branchDTO)
        {

            var response = new BaseResponseT<int>();

            #region Validation

            var ValidateChangeForMainBranchOnlyResult = BranchValidatorBL
                .ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Add);

            if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
            {
                TranslationHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                return response;
            }

            var BranchValidatorModel = new BranchValidatorModel
            {
                Branch = branchDTO,
                ChangeType = ChangeType.Add
            };

            var BranchValidatorResult = await BranchValidatorBL.BranchCreationValidator(BranchValidatorModel);

            if (BranchValidatorResult.Status != ResponseStatus.Success)
            {
                TranslationHelper.MapBaseResponse(source: BranchValidatorResult, destination: response);
                unitOfWork.Rollback();
                return response;
            }
            #endregion

            if (branchDTO.CompanyId <= 0)
            {
                branchDTO.CompanyId = requestHeaderContext.CompanyId;
            }

            var branch = BranchDTOMapper.Map(branchDTO);
            try
            {

                branchRepository.Insert(branch);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Result = 0;
                TranslationHelper.SetResponseMessages(response, "MissingData", "Missing Data", requestHeaderContext.Lang);
            }

            response.Result = branch.Id;
            return response;
        }

        public async Task<BranchDTO> GetInfo(GetBranchInfoCriteria criteria)
        {
                var branch = await branchRepository.
                    GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, nameof(Branch.Country) + DawemKeys.Comma + nameof(Branch.Currency)) ?? 
                    throw new BusinessValidationErrorException(DawemKeys.BranchNotFound);
            
            BranchDTOMapper.InitBranchContext(requestHeaderContext);
            var branchInfo = BranchDTOMapper.Map(branch);

            return branchInfo;

        }

        public async Task<GetBranchesResponseModel> Get(GetBranchesCriteria criteria)
        {
            var query = branchRepository.GetAsQueryable(criteria,nameof(Branch.Country) + DawemKeys.Comma + nameof(Branch.Currency));

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
                TotalCount= query.Count()
            };

        }
        //public async Task<BaseResponseT<object>> MasterCreate(NewBranchBindingModel newBranchBindingModel)
        //{
        //    var result = new BaseResponseT<object>
        //    {

        //    };


        //    try
        //    {

        //        unitOfWork.CreateTransaction();
        //        var createBranchResponse = await Create(newBranchBindingModel.Branch);
        //        if (createBranchResponse.Status != ResponseStatus.Success)
        //        {
        //            unitOfWork.Rollback();
        //            ResponseHelper.MapBaseResponse(source: createBranchResponse, destination: result);
        //            return result;

        //        }
        //        UserBranch userAccountSetup = new UserBranch
        //        {
        //            BranchId = createBranchResponse.Result.Id,
        //            UserId = newBranchBindingModel.UserId
        //        };

        //        var addUserToNewBranchResponse = userBranchBL.Create(userAccountSetup);

        //        if (addUserToNewBranchResponse.Status != ResponseStatus.Success)
        //        {
        //            unitOfWork.Rollback();
        //            ResponseHelper.MapBaseResponse(source: addUserToNewBranchResponse, destination: result);
        //            return result;
        //        }

        //        result.Status = ResponseStatus.Success;
        //        unitOfWork.Commit();
        //    }
        //    catch (Exception ex)
        //    {

        //        result.Exception = ex; result.Message = ex.Message;
        //        TranslationHelper.SetException(result, ex, lang: userContext.Lang);

        //    }
        //    return result;
        //}
        public async Task<BaseResponseT<bool>> Update(BranchDTO branchDTO)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {
                //unitOfWork.CreateTransaction();

                var ValidateChangeForMainBranchOnlyResult = mainBranchValidatorBL.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Edit);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }

                branchDTO.CompanyId = requestHeaderContext.CompanyId;
                var branch = BranchDTOMapper.Map(branchDTO);
                branchRepository.Update(branch);
                await unitOfWork.SaveAsync();
                //unitOfWork.Commit();
                response.Result = true;
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }
        public async Task<BaseResponseT<bool>> Delete(int Id)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {
                var ValidateChangeForMainBranchOnlyResult = mainBranchValidatorBL.ValidateChangeForMainBranchOnly(requestHeaderContext, ChangeType.Delete);
                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    return response;
                }

                branchRepository.Delete(Id);
                unitOfWork.Save();
                response.Result = true;
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.ValidationError;
                response.Result = false;
                TranslationHelper.SetResponseMessages(response, "Can'tBeDeletedItIsRelatedToOtherData", "Can't Be Deleted It Is Related To Other Data !");
            }
            return response;
        }
    }
}
