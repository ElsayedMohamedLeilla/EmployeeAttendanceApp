using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Others
{
    public class GetAllScreensWithAvailableActionsResponseModel
    {
        public GetAllScreensWithAvailableActionsResponseModel()
        {
            Screens = new List<ScreenWithAvailableActionsDTO>();
        }
        public AuthenticationType Type { get; set; }
        public string TypeName { get; set; }
        public List<ScreenWithAvailableActionsDTO> Screens { get; set; }
    }
}
