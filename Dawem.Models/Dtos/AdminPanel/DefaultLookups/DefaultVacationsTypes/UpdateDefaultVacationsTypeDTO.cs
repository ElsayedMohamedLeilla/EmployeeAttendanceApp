using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes
{
    public class UpdateDefaultVacationsTypeDTO : BaseCreateAndUpdateNameTranslation
    {
        public int Id { get; set; }
        //public int Code { get; set; }
        //public DefaultVacationType DefaultType { get; set; }
        
        [JsonIgnore]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
