using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes
{
    public class CreateDefaultShiftsTypeDTO : BaseCreateAndUpdateNameTranslation
    {

        [JsonIgnore]
        public string Name { get; set; }
        //public DefaultVacationType DefaultType { get; set; }
        public bool IsActive { get; set; }
    }
}
