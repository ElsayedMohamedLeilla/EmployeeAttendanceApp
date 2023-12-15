namespace Dawem.Models.Response.Schedules.SchedulePlans
{
    public class GetVacationBalancesResponse
    {
        public List<GetVacationBalancesResponseModel> VacationBalances { get; set; }
        public int TotalCount { get; set; }
    }
}
