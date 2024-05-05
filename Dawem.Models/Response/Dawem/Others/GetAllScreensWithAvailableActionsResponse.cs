using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Others
{
    public class GetAllScreensWithAvailableActionsResponse
    {
        public GetAllScreensWithAvailableActionsResponse()
        {
            Screens = new List<ScreenWithAvailableActionsDTO>();
        }
        public List<ScreenWithAvailableActionsDTO> Screens { get; set; }
    }
}
