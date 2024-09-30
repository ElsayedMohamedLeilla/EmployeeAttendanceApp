using Dawem.Models.Dtos.Dawem.Shared;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments
{
    public class UpdateDefaultDepartmentsDTO : BaseCreateAndUpdateNameTranslation
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
