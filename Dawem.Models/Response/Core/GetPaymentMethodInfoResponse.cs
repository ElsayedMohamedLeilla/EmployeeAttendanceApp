using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class GetPaymentMethodInfoResponse : BaseResponse
    {
        public PaymentMethodInfo? PaymentMethodInfo { get; set; }
    }
}
