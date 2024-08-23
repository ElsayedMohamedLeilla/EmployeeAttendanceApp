using Dawem.Enums.Generals;

namespace Dawem.Models.Response.AdminPanel.Screens.Screens
{
    public class GetScreenResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public string AuthenticationTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
