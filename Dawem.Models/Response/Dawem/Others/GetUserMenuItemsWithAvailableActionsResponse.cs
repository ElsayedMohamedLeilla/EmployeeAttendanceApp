using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Others
{
    public class GetUserMenuItemsWithAvailableActionsResponse
    {
        public GetUserMenuItemsWithAvailableActionsResponse()
        {
            MenuItems = new List<MenuItemWithAvailableActionsDTO>();
        }
        public List<MenuItemWithAvailableActionsDTO> MenuItems { get; set; }
    }
}
