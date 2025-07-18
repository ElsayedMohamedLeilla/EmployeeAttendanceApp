﻿using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments
{
    public class CreateDefaultDepartmentsDTO : BaseCreateAndUpdateNameTranslation
    {

        [JsonIgnore]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
