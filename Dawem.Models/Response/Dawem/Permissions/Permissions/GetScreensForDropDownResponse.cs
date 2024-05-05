namespace Dawem.Models.Response.Dawem.Attendances.FingerprintDevices
{
    public class GetScreensForDropDownResponse
    {
        public GetScreensForDropDownResponse()
        {
            Screens = new List<BaseGetForDropDownResponseModel>();
        }
        public List<BaseGetForDropDownResponseModel> Screens { get; set; }
    }
}
