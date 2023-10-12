namespace Dawem.Models.Response.Accounting
{
    public class PaymentTermSearchResult : BaseResponse
    {
        public List<PaymentTermDto>? PaymentTermResult { get; set; }

        public PaymentTermDto? Payment { get; set; }
    }
}
