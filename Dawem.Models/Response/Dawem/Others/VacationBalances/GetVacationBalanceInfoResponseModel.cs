using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Others.VacationBalances
{
    public class GetVacationBalanceInfoResponseModel
    {
        public string EmployeeName { get; set; }
        public int Code { get; set; }
        public int Year { get; set; }
        public DefaultVacationType DefaultVacationType { get; set; }
        public string DefaultVacationTypeName { get; set; }
        public float Balance { get; set; }
        public float RemainingBalance { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
