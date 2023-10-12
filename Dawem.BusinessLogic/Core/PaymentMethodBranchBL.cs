using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Data;
using SmartBusinessERP.Domain.Entities.Core;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Helpers;
using AutoMapper;
using SmartBusinessERP.Repository.Core.Conract;
using SmartBusinessERP.BusinessLogic.Core.Contract;
using SmartBusinessERP.Models.DtosMappers;

namespace SmartBusinessERP.BusinessLogic.Core
{
    public class PaymentMethodBranchBL : IPaymentMethodBranchBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IPaymentMethodBranchRepository paymentMethodBranchRepository;
        private readonly IMapper mapper;
        public PaymentMethodBranchBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IPaymentMethodBranchRepository _paymentMethodBrancheRepository,    IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            paymentMethodBranchRepository = _paymentMethodBrancheRepository;
            mapper = _mapper;
    }

   
       
        public BaseResponseT<PaymentMethodBranch> Create(PaymentMethodBranch paymentMethodBranch)
        {
            BaseResponseT<PaymentMethodBranch> response = new BaseResponseT<PaymentMethodBranch>();
            try
            {
                response.Result = paymentMethodBranchRepository.Insert(paymentMethodBranch);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }
        public BaseResponseT<List<PaymentMethodBranchDTO>> GetByPaymentMethod(int paymentMethodId)
        {
            BaseResponseT<List<PaymentMethodBranchDTO>> response = new BaseResponseT<List<PaymentMethodBranchDTO>>();
            try
            {
                var paymentMethodBranches = paymentMethodBranchRepository
                    .Get(user => user.PaymentMethodId == paymentMethodId).ToList();
                if (paymentMethodBranches != null && paymentMethodBranches.Count > 0)
                {
                    response.Result = PaymentMethodBranchDTOMapper.Map(paymentMethodBranches);
                    response.Status = ResponseStatus.Success;
                }
                else
                {
                    response.Result = null;
                    response.Status = ResponseStatus.ValidationError;
                    TranslationHelper.SetValidationMessages(response, "NoBranchForThisUser", "No Branch For This User !");

                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }
      
        public BaseResponseT<List<PaymentMethodBranch>> BulkCreate(List<PaymentMethodBranch> paymentMethodBranches)
        {
            BaseResponseT<List<PaymentMethodBranch>> response = new BaseResponseT<List<PaymentMethodBranch>>();
            try
            {
                response.Result = paymentMethodBranchRepository.BulkInsert(paymentMethodBranches).ToList();
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }

    }
}
