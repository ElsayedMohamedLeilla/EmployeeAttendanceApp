namespace Dawem.Models.Response.Dawem.Others.VacationBalances
{
    public class GetVacationBalancesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string EmployeeName { get; set; }
        public int Year { get; set; }
        public string DefaultVacationTypeName { get; set; }
        public float Balance { get; set; }
        public float RemainingBalance { get; set; }
        public bool IsActive { get; set; }
    }
}
