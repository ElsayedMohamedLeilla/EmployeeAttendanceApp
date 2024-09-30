using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles
{
    public class UpdateDefaultJobTitlesDTO : BaseCreateAndUpdateNameTranslation
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
