﻿using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes
{
    public class CreateDefaultOfficialHolidaysDTO : BaseCreateAndUpdateNameTranslation
    {

        [JsonIgnore]
        public string Name { get; set; }
        //public DefaultOfficialHolidayType DefaultType { get; set; }
        public bool IsActive { get; set; }
    }
}
