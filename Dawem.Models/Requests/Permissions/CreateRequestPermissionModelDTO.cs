﻿using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Dawem.Models.Requests.Permissions
{
    public class CreateRequestPermissionModelDTO
    {
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public int? EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<IFormFile> Attachments { get; set; }
        [JsonIgnore]
        public List<string> AttachmentsNames { get; set; }
        public string Notes { get; set; }
    }
}
