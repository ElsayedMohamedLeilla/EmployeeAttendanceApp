using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Others
{
    public class GetAllMenuItemsWithAvailableActionsResponseModel
    {
        public GetAllMenuItemsWithAvailableActionsResponseModel()
        {
            MenuItems = new List<MenuItemWithAvailableActionsDTO>();
        }
        public AuthenticationType AuthenticationType { get; set; }
        public string AuthenticationTypeName { get; set; }
        public List<MenuItemWithAvailableActionsDTO> MenuItems { get; set; }
    }
}
