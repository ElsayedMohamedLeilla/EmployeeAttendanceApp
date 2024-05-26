using Dawem.Enums.Generals;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Screens
{
    public class GetScreenResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AuthenticationType Type { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
