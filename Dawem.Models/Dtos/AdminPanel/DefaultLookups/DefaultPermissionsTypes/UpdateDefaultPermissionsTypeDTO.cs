using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes
{
    public class UpdateDefaultPermissionsTypeDTO : BaseCreateAndUpdateNameTranslation
    {
        public int Id { get; set; }
        //public int Code { get; set; }
        //public DefaultPermissionType DefaultType { get; set; }
        
        [JsonIgnore]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
