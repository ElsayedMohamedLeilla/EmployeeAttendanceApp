using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Requests.Vacations
{
    public class EmployeeGetRequestVacationsResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string VacationTypeName { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DirectManagerName { get; set; }
        public int NumberOfDays { get; set; }
        public float BalanceAfterRequest { get; set; }

    }
}
