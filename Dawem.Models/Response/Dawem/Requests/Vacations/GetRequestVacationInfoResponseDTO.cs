﻿using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Requests.Vacations
{
    public class GetRequestVacationInfoResponseDTO
    {
        public int Code { get; set; }
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public RequestEmployeeModel Employee { get; set; }
        public string VacationTypeName { get; set; }
        public int NumberOfDays { get; set; }
        public float BalanceBeforeRequest { get; set; }
        public float BalanceAfterRequest { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public List<FileDTO> Attachments { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
