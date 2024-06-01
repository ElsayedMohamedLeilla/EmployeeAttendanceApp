namespace Dawem.Models.Response.Dawem.Others
{
    public class GetAllScreensWithAvailableActionsResponse
    {
        public GetAllScreensWithAvailableActionsResponse()
        {
            ScreensTypes = new List<GetAllScreensWithAvailableActionsResponseModel>();
        }
        public List<GetAllScreensWithAvailableActionsResponseModel> ScreensTypes { get; set; }
    }
}
