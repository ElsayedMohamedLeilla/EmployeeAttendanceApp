using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class ScreenDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public GroupOrScreenType Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public GroupOrScreenType GroupOrScreenType { get; set; }
        public List<ApplicationActionCode> AvailableActions { get; set; }
        public List<MenuItemWithAvailableActionsDTO> Children { get; set; }
    }
}
