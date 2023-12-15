using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Requests.Vacations
{
    public class UpdateRequestVacationDTO
    {
        public int Id { get; set; }
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public int? EmployeeId { get; set; }
        public int VacationTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public List<string> AttachmentsNames { get; set; }
        [JsonIgnore]
        public float BalanceBeforeRequest { get; set; }
        [JsonIgnore]
        public float BalanceAfterRequest { get; set; }
    }
}
