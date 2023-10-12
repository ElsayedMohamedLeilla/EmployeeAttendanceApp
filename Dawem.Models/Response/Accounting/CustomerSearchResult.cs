namespace Dawem.Models.Response.Accounting
{
    public class CustomerSearchResult : BaseResponse
    {
        public List<CustomerDto> CustomerResult { get; set; }
        public CustomerDto Customer { get; set; }

    }
}
