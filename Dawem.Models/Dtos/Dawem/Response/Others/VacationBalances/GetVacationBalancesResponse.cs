namespace Dawem.Models.Response.Others.VacationBalances
{
    public class GetVacationBalancesResponse
    {
        public List<GetVacationBalancesResponseModel> VacationBalances { get; set; }
        public int TotalCount { get; set; }
    }
}
