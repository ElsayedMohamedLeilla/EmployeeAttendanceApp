using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Schedules.SchedulePlans
{
    public class GetVacationBalanceInfoResponseModel
    {
        public string EmployeeName { get; set; }
        public int Code { get; set; }
        public int Year { get; set; }
        public VacationType VacationType { get; set; }
        public string VacationTypeName { get; set; }
        public float Balance { get; set; }
        public float RemainingBalance { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
