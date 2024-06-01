using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Others
{
    public class OldGetAllScreensWithAvailableActionsResponse
    {
        public OldGetAllScreensWithAvailableActionsResponse()
        {
            Screens = new List<OldScreenWithAvailableActionsDTO>();
        }
        public List<OldScreenWithAvailableActionsDTO> Screens { get; set; }
    }
}
