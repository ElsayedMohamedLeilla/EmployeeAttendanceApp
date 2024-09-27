namespace Dawem.Models.Response.Dawem.Permissions.Permissions
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
