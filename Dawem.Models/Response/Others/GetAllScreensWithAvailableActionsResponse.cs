using Dawem.Models.Dtos.Others;

namespace Dawem.Models.Response.Others
{
    public class GetAllScreensWithAvailableActionsResponse : BaseResponse
    {
        public GetAllScreensWithAvailableActionsResponse()
        {
            Screens = new List<ScreensWithAvailableActionsDTO>();
        }
        public List<ScreensWithAvailableActionsDTO> Screens { get; set; }
    }
}
