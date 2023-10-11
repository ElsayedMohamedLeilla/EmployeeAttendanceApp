
using SmartBusinessERP.Domain.Entities.Accounts;
using SmartBusinessERP.Models.Response;


namespace Glamatek.Model.SearchResults.Account
{
    public class BankAccountSearchResult : BaseResponse
    {
        public List<BankAccountDto> BankAccountResult { get; set; }


    }
}
