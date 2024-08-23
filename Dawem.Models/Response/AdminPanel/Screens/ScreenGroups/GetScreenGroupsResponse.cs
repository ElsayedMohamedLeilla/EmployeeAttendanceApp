namespace Dawem.Models.Response.AdminPanel.Screens.ScreenGroups
{
    public class GetScreenGroupsResponse
    {
        public List<GetScreenGroupResponseModel> ScreenGroups { get; set; }
        public int TotalCount { get; set; }
    }
}
