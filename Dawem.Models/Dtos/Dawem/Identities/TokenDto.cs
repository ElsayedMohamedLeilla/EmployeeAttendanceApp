using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Dtos.Dawem.Identities
{
    public class TokenDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public List<MenuItemWithAvailableActionsDTO> MenuItems { get; set; }
    }
}
