using AutoMapper;
using SmartBusinessERP.BusinessLogic.Core.Contract;
using SmartBusinessERP.BusinessLogic.Validators.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Inventory;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.DtosMappers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Repository.Core.Conract;

namespace SmartBusinessERP.BusinessLogic.Core
{
    public class StoreBL : IStoreBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IStoreRepository storeRepository;
        private readonly IBranchValidatorBL branchValidatorBL;
        private readonly IMapper mapper;
        private readonly RequestHeaderContext userContext;


        public StoreBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IStoreRepository _storeRepository, RequestHeaderContext _userContext,
            IBranchValidatorBL _branchValidatorBL, IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            branchValidatorBL = _branchValidatorBL;
            storeRepository = _storeRepository;
            mapper = _mapper;
            userContext = _userContext;
        }

        public async Task<BaseResponseT<StoreDTO>> GetById(int Id)
        {
            BaseResponseT<StoreDTO> response = new();
            try
            {
                var result = storeRepository.Get(x => x.Id == Id).FirstOrDefault();
                if (result == null)
                {
                    TranslationHelper.SetResponseMessages(response, "NoDataFound", "No Data Found", "");
                    response.Status = ResponseStatus.Error;
                }
                else
                {
                    response.Result = StoreDTOMapper.Map(result);
                    response.Status = ResponseStatus.Success;
                }               
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }

        public async Task<GetStoresResponse> Get(GetStoresCriteria criteria)
        {
            var result = new GetStoresResponse();

            try
            {
                var query = storeRepository.GetAsQueryable(criteria);
                var queryOrdered = storeRepository.OrderBy(query, "Id", "desc");

                #region paging

                var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                var take = PagingHelper.Take(criteria.PageSize);

                var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                var stores = queryPaged.ToList();
                StoreDTOMapper.InitStoreContext(userContext);
                result.Stores = StoreDTOMapper.Map(stores);
                result.Status = ResponseStatus.Success;
                result.TotalCount = query.Count();
            }
            catch (Exception ex)
            {
                result.Exception = ex; result.Message = ex.Message;
                result.Status = ResponseStatus.Error;
            }
            return result;
        }

        public async Task<GetStoreInfoResponse> GetInfo(GetStoreInfoCriteria criteria)
        {

            GetStoreInfoResponse storeSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                var store = await storeRepository.GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id);

                if (store != null)
                {
                    StoreDTOMapper.InitStoreContext(userContext);
                    var storeInfo = StoreDTOMapper.MapInfo(store);
                    storeSearchResult.StoreInfo = storeInfo;
                    storeSearchResult.Status = ResponseStatus.Success;
                }
                else
                {
                    storeSearchResult.Status = ResponseStatus.ValidationError;
                    TranslationHelper
                    .SetResponseMessages
                        (storeSearchResult, "StoreNotFound!",
                        "Store Not Found !", lang: userContext.Lang);
                }
            }
            catch (Exception ex)
            {
                storeSearchResult.Exception = ex;
                storeSearchResult.Status = ResponseStatus.Error;
            }
            return storeSearchResult;

        }

        public async Task<BaseResponseT<Store>> Create(Store store)
        {
            var response = new BaseResponseT<Store>();
            try
            {
                unitOfWork.CreateTransaction();

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Add);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }


                var ValidateStoreResult = ValidateStore(store);

                if (ValidateStoreResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateStoreResult, destination: response);
                    return response;
                }
                store.MainBranchId = userContext.BranchId ?? 0;
                response.Result = storeRepository.Insert(store);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }

        public BaseResponseT<bool> ValidateStore(Store store)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();

            try
            {
                response.Status = ResponseStatus.Success;

                if (store.StoreType <= 0)
                {
                    response.Status = ResponseStatus.ValidationError;
                    TranslationHelper.SetResponseMessages(response, "ChooseStoreType!", "Choose Payment Method Type !", lang: userContext.Lang);
                    return response;
                }


                var getStore = storeRepository
                    .Get(pm => pm.MainBranchId == store.MainBranchId && pm.StoreType == StoreType.MainStore && pm.StoreType ==
                    store.StoreType && pm.Id != store.Id).ToList();

                if (getStore != null && getStore.Count() > 0)
                {

                    response.Status = ResponseStatus.ValidationError;


                    TranslationHelper.SetResponseMessages(response,
                        "StoreTypeCanNotRepeated!",
                        "Payment Method Type Can Not Repeated !", lang: userContext.Lang);

                    return response;
                }


                return response;
            }
            catch (Exception ex)
            {

                TranslationHelper.SetException(response, ex);
                return response;
            }
        }

        public async Task<BaseResponseT<bool>> Update(Store store)
        {
            var response = new BaseResponseT<bool>();
            try
            {
                unitOfWork.CreateTransaction();

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Edit);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    TranslationHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }


                var ValidateStoreResult = ValidateStore(store);

                if (ValidateStoreResult.Status != ResponseStatus.Success)
                {
                    TranslationHelper.MapBaseResponse(source: ValidateStoreResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }

                store.MainBranchId = userContext.BranchId ?? 0;
                storeRepository.Update(store);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }

            response.Status = ResponseStatus.Success;
            return response;

        }

        public async Task<BaseResponseT<bool>> Delete(int Id)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Delete);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    return response;
                }


                await unitOfWork.SaveAsync();
                storeRepository.Delete(Id);
                await unitOfWork.SaveAsync();
                response.Result = true;
                response.Status = ResponseStatus.Success;
            }

            catch (Exception)
            {
                response.Status = ResponseStatus.ValidationError;
                response.Result = false;
                TranslationHelper.SetResponseMessages(response, "Can'tBeDeletedItIsRelatedToOtherData", "Can't Be Deleted It Is Related To Other Data !");

            }

            return response;
        }
    }

}

