
using SmartBusinessERP.Domain.Entities.Accounts;
using SmartBusinessERP.Models.Response;


namespace Glamatek.Model.SearchResults.Account
{
    public class CustomerSearchResult : BaseResponse
    {
        public List<CustomerDto> CustomerResult { get; set; }
        public CustomerDto Customer { get; set; }

    }
}
