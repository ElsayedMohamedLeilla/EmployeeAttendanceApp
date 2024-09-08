using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes
{
    public class UpdateDefaultOfficialHolidaysDTO : BaseCreateAndUpdateNameTranslation
    {
        public int Id { get; set; }
        //public int Code { get; set; }
        //public DefaultOfficialHolidayType DefaultType { get; set; }

        [JsonIgnore]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
