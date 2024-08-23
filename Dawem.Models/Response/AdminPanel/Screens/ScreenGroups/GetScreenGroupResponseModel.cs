namespace Dawem.Models.Response.AdminPanel.Screens.ScreenGroups
{
    public class GetScreenGroupResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string AuthenticationTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
