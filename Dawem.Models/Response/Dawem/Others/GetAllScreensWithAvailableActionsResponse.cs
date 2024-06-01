namespace Dawem.Models.Response.Dawem.Others
{
    public class GetAllScreensWithAvailableActionsResponse
    {
        public GetAllScreensWithAvailableActionsResponse()
        {
            MenuItemsTypes = new List<GetAllMenuItemsWithAvailableActionsResponseModel>();
        }
        public List<GetAllMenuItemsWithAvailableActionsResponseModel> MenuItemsTypes { get; set; }
    }
}
