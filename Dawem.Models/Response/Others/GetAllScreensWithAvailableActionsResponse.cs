using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Others
{
    public class GetAllScreensWithAvailableActionsResponse
    {
        public GetAllScreensWithAvailableActionsResponse()
        {
            Screens = new List<ScreensWithAvailableActionsDTO>();
        }
        public List<ScreensWithAvailableActionsDTO> Screens { get; set; }
    }
}
