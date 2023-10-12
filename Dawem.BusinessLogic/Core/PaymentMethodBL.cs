using AutoMapper;
using SmartBusinessERP.BusinessLogic.Core.Contract;
using SmartBusinessERP.BusinessLogic.Validators.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Core;
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
    public class PaymentMethodBL : IPaymentMethodBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IPaymentMethodBranchRepository paymentMethodBranchRepository;
        private readonly IBranchValidatorBL branchValidatorBL;
        private readonly IPaymentMethodBranchBL paymentMethodBranchBL;
        private readonly IMapper mapper;
        private readonly RequestHeaderContext userContext;
        public PaymentMethodBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IPaymentMethodRepository _paymentMethodRepository, RequestHeaderContext _userContext,
            IBranchValidatorBL _branchValidatorBL, IPaymentMethodBranchBL _paymentMethodBranchBL, IPaymentMethodBranchRepository _paymentMethodBranchRepository, IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            paymentMethodBranchBL = _paymentMethodBranchBL;
            branchValidatorBL = _branchValidatorBL;
            paymentMethodRepository = _paymentMethodRepository;
            paymentMethodBranchRepository = _paymentMethodBranchRepository;
            mapper = _mapper;
            userContext = _userContext;
        }
        public async Task<BaseResponseT<PaymentMethodDTO>> GetById(int Id)
        {
            BaseResponseT<PaymentMethodDTO> response = new();
            try
            {
                var result = paymentMethodRepository.Get(x => x.Id == Id).FirstOrDefault();
                if (result == null)
                {
                    TranslationHelper.SetResponseMessages(response, "NoDataFound", "No Data Found", "");
                    response.Status = ResponseStatus.Error;
                }
                else
                {
                    response.Result = PaymentMethodDTOMapper.Map(result);
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
        public BaseResponseT<PaymentMethodDTO> GetBranchCashPaymentMethod(int branchId)
        {
            BaseResponseT<PaymentMethodDTO> response = new();
            try
            {
                var result = paymentMethodRepository.GetEntityByCondition(x =>
                x.PaymentMethodBranches.Any(pmb => pmb.BranchId == branchId) &&
                x.PaymentMethodType == PaymentMethodType.Cash);


                if (result == null)
                {
                    TranslationHelper.SetResponseMessages(
                            response,
                            "CannotFindCashPaymentMethodForCurrentBranch!",
                            "Cannot Find Cash Payment Method For Current Branch !");
                    response.Status = ResponseStatus.Error;
                }
                else
                {
                    response.Result = PaymentMethodDTOMapper.Map(result);
                    response.Status = ResponseStatus.Success;
                }

            }
            catch (Exception ex)
            {
                //response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }
        public async Task<PaymentMethodSearchResult> Get(PaymentMethodSearchCriteria criteria)
        {
            var result = new PaymentMethodSearchResult();

            try
            {

                var query = paymentMethodRepository.GetAsQueryable(criteria, includeProperties: !userContext.IsMainBranch && !criteria.ForGridView ? "PaymentMethodBranches" : "");
                var queryOrdered = paymentMethodRepository.OrderBy(query, "Id", "desc");

                #region paging

                var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                var take = PagingHelper.Take(criteria.PageSize);

                var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                var paymentMethods = queryPaged.ToList();
                PaymentMethodDTOMapper.InitPaymentMethodContext(userContext);
                result.PaymentMethods = PaymentMethodDTOMapper.Map(paymentMethods);
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
        public async Task<GetPaymentMethodInfoResponse> GetInfo(GetPaymentMethodInfoCriteria criteria)
        {

            GetPaymentMethodInfoResponse paymentMethodSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                var paymentMethod = await paymentMethodRepository.GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, "PaymentMethodBranches,PaymentMethodBranches.Branch");

                if (paymentMethod != null)
                {
                    PaymentMethodDTOMapper.InitPaymentMethodContext(userContext);
                    PaymentMethodBranchDTOMapper.InitUserContext(userContext);
                    var paymentMethodInfo = PaymentMethodDTOMapper.MapInfo(paymentMethod);
                    paymentMethodSearchResult.PaymentMethodInfo = paymentMethodInfo;
                    paymentMethodSearchResult.Status = ResponseStatus.Success;
                }
                else
                {
                    paymentMethodSearchResult.Status = ResponseStatus.ValidationError;
                    TranslationHelper
                    .SetResponseMessages
                        (paymentMethodSearchResult, "PaymentMethodNotFound!",
                        "PaymentMethod Not Found !", lang: userContext.Lang);
                }


            }
            catch (Exception ex)
            {
                paymentMethodSearchResult.Exception = ex;
                paymentMethodSearchResult.Status = ResponseStatus.Error;
            }
            return paymentMethodSearchResult;

        }
        public async Task<BaseResponseT<PaymentMethod>> Create(PaymentMethod paymentMethod)
        {
            var response = new BaseResponseT<PaymentMethod>();
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


                var ValidatePaymentMethodResult = ValidatePaymentMethod(paymentMethod);

                if (ValidatePaymentMethodResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidatePaymentMethodResult, destination: response);
                    return response;
                }
                paymentMethod.MainBranchId = userContext.BranchId ?? 0;
                response.Result = paymentMethodRepository.Insert(paymentMethod);
                unitOfWork.Save();
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
        public BaseResponseT<bool> ValidatePaymentMethod(PaymentMethod paymentMethod)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();

            try
            {
                response.Status = ResponseStatus.Success;

                if (paymentMethod.PaymentMethodType <= 0)
                {
                    response.Status = ResponseStatus.ValidationError;
                    TranslationHelper.SetResponseMessages(response, "ChoosePaymentMethodType!", "Choose Payment Method Type !", lang: userContext.Lang);
                    return response;
                }


                var getPaymentMethod = paymentMethodRepository
                    .Get(pm => pm.MainBranchId == paymentMethod.MainBranchId && pm.PaymentMethodType ==
                    paymentMethod.PaymentMethodType && pm.Id != paymentMethod.Id &&
                    ((pm.IsDefault == paymentMethod.IsDefault && paymentMethod.IsDefault != null) || (paymentMethod.IsDefault == null))).ToList();

                if (getPaymentMethod != null && getPaymentMethod.Count() > 0)
                {

                    response.Status = ResponseStatus.ValidationError;

                    if (paymentMethod.IsDefault == true)
                    {
                        TranslationHelper.SetResponseMessages(response,
                            "PaymentMethodIsDefaultNotRepeated",
                            "You couldn't set this payment method as default because there is another default payment method !", lang: userContext.Lang);
                    }
                    else
                    {
                        TranslationHelper.SetResponseMessages(response,
                            "PaymentMethodTypeCanNotRepeated!",
                            "Payment Method Type Can Not Repeated !", lang: userContext.Lang);
                    }
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
        public async Task<BaseResponseT<bool>> Update(PaymentMethod paymentMethod)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
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


                var ValidatePaymentMethodResult = ValidatePaymentMethod(paymentMethod);

                if (ValidatePaymentMethodResult.Status != ResponseStatus.Success)
                {
                    TranslationHelper.MapBaseResponse(source: ValidatePaymentMethodResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }

                var PaymentMethodBranches = paymentMethod.PaymentMethodBranches;
                paymentMethod.PaymentMethodBranches = null;
                paymentMethod.MainBranchId = userContext.BranchId ?? 0;
                paymentMethodRepository.Update(paymentMethod);
                unitOfWork.Save();

                #region Payment Method Branche


                var willDeletePaymentMethodBranch = paymentMethodBranchBL
                        .GetByPaymentMethod(paymentMethod.Id);

                if (willDeletePaymentMethodBranch.Result != null)
                {
                    foreach (var paymentMethodBranch in willDeletePaymentMethodBranch.Result)
                    {
                        paymentMethodBranchRepository.Delete(paymentMethodBranch.Id);
                    }
                }


                if (PaymentMethodBranches != null && PaymentMethodBranches.Count() > 0)
                {
                    List<PaymentMethodBranch> paymentMethodBranches = new List<PaymentMethodBranch>();

                    foreach (var paymentMethodBranch in PaymentMethodBranches)
                    {
                        if (paymentMethodBranch.Id > 0)
                        {
                            paymentMethodBranchRepository.Update(paymentMethodBranch);
                        }
                        else
                        {
                            paymentMethodBranchRepository.Insert(paymentMethodBranch);
                        }



                        unitOfWork.Save();

                        response.Result = true;
                        response.Status = ResponseStatus.Success;
                    }
                }

                #endregion

                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
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

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Delete);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    return response;
                }

                var paymentMethodBranches = paymentMethodBranchRepository.Get(a => a.PaymentMethodId == Id).ToList();

                foreach (var item in paymentMethodBranches)
                {
                    paymentMethodBranchRepository.Delete(item);
                }
                unitOfWork.Save();
                paymentMethodRepository.Delete(Id);
                unitOfWork.Save();
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

