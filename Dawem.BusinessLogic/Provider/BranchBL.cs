using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartBusinessERP.BusinessLogic.Provider.Contract;
using SmartBusinessERP.BusinessLogic.Validators;
using SmartBusinessERP.BusinessLogic.Validators.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Provider;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Criteria.Provider;
using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Models.DtosMappers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Models.Response.Provider;
using SmartBusinessERP.Repository.Provider.Contract;

namespace SmartBusinessERP.BusinessLogic.Provider
{
    public class BranchBL : IBranchBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IBranchRepository branchRepository;
        private readonly RequestHeaderContext userContext;
        private readonly IBranchValidatorBL BranchValidatorBL;
        private readonly IBranchValidatorBL mainBranchValidatorBL;
        private readonly IMapper mapper;
        private readonly IUserBranchBL userBranchBL;

        public BranchBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IBranchValidatorBL _mainBranchValidatorBL,
               RequestHeaderContext _userContext, IBranchRepository _branchRepository,
               IMapper _mapper, IBranchValidatorBL _BranchValidatorBL, IUserBranchBL _userBranchBL)
        {
            unitOfWork = _unitOfWork;
            userContext = _userContext;
            mainBranchValidatorBL = _mainBranchValidatorBL;
            BranchValidatorBL = _BranchValidatorBL;
            branchRepository = _branchRepository;
            mapper = _mapper;
            userBranchBL = _userBranchBL;
        }

        public async Task<BaseResponseT<int>> Create(BranchDTO branchDTO)
        {

            var response = new BaseResponseT<int>();

            #region Validation

            var ValidateChangeForMainBranchOnlyResult = BranchValidatorBL
                .ValidateChangeForMainBranchOnly(userContext, ChangeType.Add);

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
                branchDTO.CompanyId = userContext.CompanyId;
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
                response.Result =0;
                TranslationHelper.SetResponseMessages(response, "MissingData", "Missing Data", userContext.Lang);
            }

            response.Result = branch.Id;
            return response;
        }

        public async Task<GetBranchInfoResponse> GetInfo(GetBranchInfoCriteria criteria)
        {

            GetBranchInfoResponse unitSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                var branch = await branchRepository.GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, "Country,Currency");

                if (branch != null)
                {
                    BranchDTOMapper.InitBranchContext(userContext);
                    var branchInfo = BranchDTOMapper.Map(branch);
                    unitSearchResult.BranchInfo = branchInfo;
                    unitSearchResult.Status = ResponseStatus.Success;
                }
                else
                {
                    unitSearchResult.Status = ResponseStatus.ValidationError;
                    TranslationHelper
                    .SetResponseMessages
                        (unitSearchResult, "BranchNotFound!",
                        "Branch Not Found !", lang: userContext.Lang);
                }
            }
            catch (Exception ex)
            {
                unitSearchResult.Exception = ex;
                unitSearchResult.Status = ResponseStatus.Error;
            }
            return unitSearchResult;

        }

        public async Task<GetBranchesResponse> Get(GetBranchesCriteria criteria)
        {

            GetBranchesResponse getBranchesResponse = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                ExpressionStarter<Branch> branchPredicate = PredicateBuilder.New<Branch>(true);


                if (criteria.Id > 0)
                {
                    branchPredicate = branchPredicate.And(x => x.Id == criteria.Id);
                }

                if (!string.IsNullOrWhiteSpace(criteria.FreeText))
                {
                    criteria.FreeText = criteria.FreeText.ToLower().Trim();

                    branchPredicate = branchPredicate.Start(x => x.BranchName.ToLower().Trim().Contains(criteria.FreeText));
                }

                branchPredicate = branchPredicate.And(x => x.CompanyId == userContext.CompanyId);

                #region paging

                int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                int take = PagingHelper.Take(criteria.PageSize);

                var query = branchRepository.Get(branchPredicate, IncludeProperties: "Country,Currency");

                #region sorting

                var queryOrdered = branchRepository.OrderBy(query, "Id", "desc");

                #endregion

                var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                var branches = await queryPaged.ToListAsync();

                BranchDTOMapper.InitBranchContext(userContext);
                getBranchesResponse.Branches = BranchDTOMapper.Map(branches);
                getBranchesResponse.TotalCount = queryOrdered.ToList().Count;
                getBranchesResponse.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                getBranchesResponse.Exception = ex;
                getBranchesResponse.Status = ResponseStatus.Error;
            }
            return getBranchesResponse;

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

                var ValidateChangeForMainBranchOnlyResult = mainBranchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Edit);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }

                branchDTO.CompanyId = userContext.CompanyId;
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
                var ValidateChangeForMainBranchOnlyResult = mainBranchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Delete);
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
