using SmartBusinessERP.Domain.Entities.Core;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;

namespace SmartBusinessERP.BusinessLogic.Core.Contract
{
    public interface IPaymentMethodBL
    {
        Task<BaseResponseT<PaymentMethodDTO>> GetById(int Id);
        Task<PaymentMethodSearchResult> Get(PaymentMethodSearchCriteria criteria);
        Task<GetPaymentMethodInfoResponse> GetInfo(GetPaymentMethodInfoCriteria criteria);
        Task<BaseResponseT<PaymentMethod>> Create(PaymentMethod paymentMethod);
        Task<BaseResponseT<bool>> Update(PaymentMethod paymentMethod);
        Task<BaseResponseT<bool>> Delete(int Id);
    }
}
