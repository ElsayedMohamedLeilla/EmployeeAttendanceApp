using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class GetPaymentMethodInfoResponse : BaseResponse
    {
        public PaymentMethodInfo? PaymentMethodInfo { get; set; }
    }
}
