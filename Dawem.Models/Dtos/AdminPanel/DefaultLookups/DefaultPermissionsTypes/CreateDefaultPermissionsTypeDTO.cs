using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes
{
    public class CreateDefaultPermissionsTypeDTO : BaseCreateAndUpdateNameTranslation
    {

        [JsonIgnore]
        public string Name { get; set; }
        //public DefaultPermissionType DefaultType { get; set; }
        public bool IsActive { get; set; }
    }
}
