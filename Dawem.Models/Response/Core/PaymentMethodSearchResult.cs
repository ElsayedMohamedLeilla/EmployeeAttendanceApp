using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class PaymentMethodSearchResult :  BaseResponse
    {
        public List<PaymentMethodDTO> PaymentMethods { get; set; }
    }
}
