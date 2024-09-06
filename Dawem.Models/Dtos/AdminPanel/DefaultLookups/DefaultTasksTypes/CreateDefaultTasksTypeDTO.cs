using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes
{
    public class CreateDefaultTasksTypeDTO : BaseCreateAndUpdateNameTranslation
    {

        [JsonIgnore]
        public string Name { get; set; }
        //public DefaultTaskType DefaultType { get; set; }
        public bool IsActive { get; set; }
    }
}
