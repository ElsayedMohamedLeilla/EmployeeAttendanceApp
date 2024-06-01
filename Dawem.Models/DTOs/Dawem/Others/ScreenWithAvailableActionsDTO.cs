using Dawem.Enums.Permissions;
using Newtonsoft.Json;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class ScreenWithAvailableActionsDTO
    {  
        public int Id { get; set; }
        public GroupOrScreenType Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        [JsonIgnore]
        public string URL { get; set; }
        public List<ApplicationActionCode> AvailableActions { get; set; }
        public List<ScreenWithAvailableActionsDTO> Children { get; set; }
    }
}
