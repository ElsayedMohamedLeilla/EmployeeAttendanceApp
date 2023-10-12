using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class PaymentMethodSearchResult : BaseResponse
    {
        public List<PaymentMethodDTO> PaymentMethods { get; set; }
    }
}
