using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Requests.Vacations
{
    public class GetRequestVacationsResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public RequestEmployeeModel Employee { get; set; }
        public string VacationTypeName { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float BalanceAfterRequest { get; set; }
    }
}
