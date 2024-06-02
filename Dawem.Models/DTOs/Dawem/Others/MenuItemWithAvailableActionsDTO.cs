using Dawem.Enums.Permissions;
using Newtonsoft.Json;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class MenuItemWithAvailableActionsDTO
    {  
        public int Id { get; set; }
        public GroupOrScreenType GroupOrScreenType { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        [JsonIgnore]
        public string URL { get; set; }
        public List<ApplicationActionCode> AvailableActions { get; set; }
        public List<MenuItemWithAvailableActionsDTO> Children { get; set; }
    }
}
