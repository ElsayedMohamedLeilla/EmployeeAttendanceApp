namespace Dawem.Models.Response.Dawem.Attendances.FingerprintDevices
{
    public class GetActionsForDropDownResponse
    {
        public GetActionsForDropDownResponse()
        {
            Actions = new List<BaseGetForDropDownResponseModel>();
        }
        public List<BaseGetForDropDownResponseModel> Actions { get; set; }
    }
}
