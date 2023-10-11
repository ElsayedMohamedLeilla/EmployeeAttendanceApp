
using SmartBusinessERP.Domain.Entities.Accounts;
using SmartBusinessERP.Models.Response;


namespace Glamatek.Model.SearchResults.Account
{
    public class PaymentTermSearchResult : BaseResponse
    {
        public List<PaymentTermDto>? PaymentTermResult { get; set; }

        public PaymentTermDto? Payment { get; set; }
    }
}
