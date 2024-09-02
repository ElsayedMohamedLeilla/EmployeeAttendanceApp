using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes
{
    public class UpdateDefaultJustificationsTypeDTO : BaseCreateAndUpdateNameTranslation
    {
        public int Id { get; set; }
        //public int Code { get; set; }
        //public DefaultVacationType DefaultType { get; set; }

        [JsonIgnore]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
