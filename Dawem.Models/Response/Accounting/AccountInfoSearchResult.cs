
using SmartBusinessERP.Models.Dtos.Accounting;



namespace SmartBusinessERP.Models.Response.Accounting
{
  public  class AccountInfoSearchResult : BaseResponseT<AccountInfoDTO>
    {
        public List<AccountInfoDTO>?  AccountInfos { get; set; }

        public AccountInfoDTO? AccountInfo { get; set; }
    }
}
