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
using SmartBusinessERP.Models.Dtos.Shared;
using SmartBusinessERP.Models.DtosMappers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Repository.Core.Conract;

namespace SmartBusinessERP.BusinessLogic.Core
{
    public class UnitBL : IUnitBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;

        private readonly IUnitBranchBL unitBranchBL;
        private readonly IUnitBranchRepository unitBranchRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IProductUnitRepository productUnitRepository;
        private readonly IBranchValidatorBL mainBranchValidatorBL;
        private readonly IMapper mapper;
        private readonly RequestHeaderContext userContext;
        public UnitBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IUnitRepository _unitRepository,
            IUnitBranchRepository _unitBranchRepository,
            IUnitBranchBL _unitBranchBL,

            IProductUnitRepository _productUnitRepository,  IMapper _mapper, IBranchValidatorBL _mainBranchValidatorBL, RequestHeaderContext _userContext)
        {
            unitOfWork = _unitOfWork;
            unitBranchRepository = _unitBranchRepository;
            unitBranchBL = _unitBranchBL;
            unitRepository = _unitRepository;
            mapper = _mapper;
            productUnitRepository = _productUnitRepository;
            mainBranchValidatorBL = _mainBranchValidatorBL;
            userContext = _userContext;
        }

        public async Task<BaseResponseT<UnitDTO>> GetById(int Id)
        {
            BaseResponseT<UnitDTO> response = new();
            try
            {
                response.Result = mapper.Map<UnitDTO>(unitRepository.GetByID(Id));
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                //response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }
        public async Task<UnitSearchResult> Get(UnitSearchCriteria criteria)
        {
            var unitSearchResult = new UnitSearchResult();

            try
            {
                
                var query = unitRepository.GetAsQueryable(criteria,includeProperties:!userContext.IsMainBranch && !criteria.ForGridView ? "PaymentMethodBranches" : "");
                var queryWithProductId = query.ToList();
                var units = new List<Unit>();
                if (criteria.ProductId != null)
                {
                    var unitIds = productUnitRepository.Get(c => c.ItemId == criteria.ProductId).Select(c => c.UnitId);
                    foreach (var u in queryWithProductId)
                    {
                        foreach (var uId in unitIds)
                        {
                            if (u.Id == uId)
                            {
                                units.Add(u);
                            }
                        }
                    }
                }
                else
                {
                    #region paging

                    var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                    var take = PagingHelper.Take(criteria.PageSize);

                    var queryOrdered = unitRepository.OrderBy(query, "Id", "desc");

                    var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                    units = queryPaged.ToList();

                    #endregion
                }

                unitSearchResult.Units = UnitDTOMapper.Map(units);
                unitSearchResult.TotalCount = units.Count();
                unitSearchResult.Status = ResponseStatus.Success;
            }

            catch (Exception ex)
            {
                unitSearchResult.Status = ResponseStatus.Error;
                unitSearchResult.Exception = ex;
            }
            return unitSearchResult;
        }
        public async Task<GetUnitInfoResponse> GetInfo(GetUnitInfoCriteria criteria)
        {

            GetUnitInfoResponse unitSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                var unit = await unitRepository.GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, "UnitBranches,UnitBranches.Branch");

                if (unit != null)
                {
                    UnitDTOMapper.InitUnitContext(userContext);
                    var unitInfo = UnitDTOMapper.MapInfo(unit);
                    unitSearchResult.UnitInfo = unitInfo;
                    unitSearchResult.Status = ResponseStatus.Success;
                }
                else
                {
                    unitSearchResult.Status = ResponseStatus.ValidationError;
                    TranslationHelper
                    .SetResponseMessages
                        (unitSearchResult, "UnitNotFound!",
                        "Unit Not Found !", lang: userContext.Lang);
                }


            }
            catch (Exception ex)
            {
                unitSearchResult.Exception = ex;
                unitSearchResult.Status = ResponseStatus.Error;
            }
            return unitSearchResult;

        }
        public async Task<BaseResponseT<Unit>> Create(Unit unit)
        {
            var response = new BaseResponseT<Unit>();
            try
            {
                unit.MainBranchId = userContext.BranchId ?? 0;
                unitOfWork.CreateTransaction();
                var ValidateChangeForMainBranchOnlyResult = mainBranchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Add);
                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }


                response.Result = unitRepository.Insert(unit);
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
        public async Task<BaseResponseT<bool>> Update(Unit unit)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {
                unitOfWork.CreateTransaction();

                var ValidateChangeForMainBranchOnlyResult = mainBranchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Edit);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }
                var UnitBranches = unit.UnitBranches;
                unit.UnitBranches = null;
                unit.MainBranchId = userContext.BranchId ?? 0;
                unitRepository.Update(unit);
                unitOfWork.Save();

                #region Unit Branches


                var willDeleteUnitBranch = unitBranchBL
                        .GetByUnit(unit.Id);

                if (willDeleteUnitBranch.Result != null)
                {
                    foreach (var UnitBranch in willDeleteUnitBranch.Result)
                    {
                        unitBranchRepository.Delete((int)UnitBranch.Id);
                    }
                }


                if (UnitBranches != null && UnitBranches.Count() > 0)
                {
                    
                    foreach (var unitBranch in UnitBranches)
                    {
                        if (unitBranch.Id > 0)
                        {
                            var createUnitBranchResponse = unitBranchBL
                                                    .Update(unitBranch);
                            if (createUnitBranchResponse.Status != ResponseStatus.Success)
                            {
                                ResponseHelper.MapBaseResponse(source: createUnitBranchResponse, destination: response);
                                unitOfWork.Rollback();
                                return response;
                            }
                            
                        }
                        else
                        {
                            var createUnitBranchResponse = unitBranchBL
                                                    .Create(unitBranch);
                            if (createUnitBranchResponse.Status != ResponseStatus.Success)
                            {
                                ResponseHelper.MapBaseResponse(source: createUnitBranchResponse, destination: response);
                                unitOfWork.Rollback();
                                return response;
                            }
                        }                      
                    }                    
                }

                #endregion

                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
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

                var productUnits = productUnitRepository.Get(c => c.UnitId == Id).FirstOrDefault();

                if (productUnits != null)
                {
                    response.Result = false;
                    response.Status = ResponseStatus.ValidationError;
                    TranslationHelper.SetResponseMessages(response, "UnDH1000", "This unit is retated to product !");
                }
                else
                {

                    var unitBranches = unitBranchRepository.Get(a => a.UnitId == Id).ToList();

                    foreach (var item in unitBranches)
                    {
                        unitBranchRepository.Delete(item);
                    }
                    await unitOfWork.SaveAsync();
                    unitRepository.Delete(Id);
                    await unitOfWork.SaveAsync();
                    response.Result = true;
                    response.Status = ResponseStatus.Success;
                }

            }

            catch (Exception ex)

            {
                response.Status = ResponseStatus.ValidationError;
                response.Result = false;
                TranslationHelper.SetResponseMessages(response, "Can'tBeDeletedItIsRelatedToOtherData", "Can't Be Deleted It Is Related To Other Data !");
            }
            return response;
        }
        public BaseResponseT<bool> IsNameUnique(ValidationItems validationItem)
        {
            BaseResponseT<bool> response = new();
            try
            {
                //string currentName;
                Unit duplicateUnit = null;
                if (string.IsNullOrEmpty(validationItem.Item))
                {
                    response.Result = true;
                    response.Status = ResponseStatus.Success;
                    return response;
                }
                if (validationItem.validationMode == ValidationMode.Create)
                {
                    duplicateUnit = unitRepository.Get(x => (x.NameEn.ToLower().Trim() ?? x.NameAr.ToLower().Trim()) == validationItem.Item.ToLower().Trim()).FirstOrDefault();
                }

                else if (validationItem.validationMode == ValidationMode.Update && validationItem.Id != null)
                {
                    duplicateUnit = unitRepository.Get(x => (x.NameEn.ToLower() ?? x.NameAr.ToLower()) == validationItem.Item.ToLower() && x.Id != validationItem.Id.Value).FirstOrDefault();
                }

                response.Result = duplicateUnit == null;
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
    }
}
