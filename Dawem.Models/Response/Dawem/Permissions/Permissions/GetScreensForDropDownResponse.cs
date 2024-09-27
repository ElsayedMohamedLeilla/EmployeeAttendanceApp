namespace Dawem.Models.Response.Dawem.Permissions.Permissions
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
