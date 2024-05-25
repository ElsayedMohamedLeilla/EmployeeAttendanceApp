using Dawem.Enums.Generals;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Screens
{
    public class GetScreenGroupResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ScreenGroupType GroupType { get; set; }
        public string GroupTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
