﻿using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Requests.Tasks
{
    public class GetRequestTaskInfoResponseModel
    {
        public int Code { get; set; }
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public RequestEmployeeModel Employee { get; set; }
        public string TaskTypeName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string StatusName { get; set; }
        public RequestStatus Status { get; set; }
        public List<string> TaskEmployees { get; set; }
        public List<FileDTO> Attachments { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
