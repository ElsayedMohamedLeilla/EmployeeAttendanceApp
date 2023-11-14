using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Others
{
    public class UserScreenActionPermissionDTO
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
        public ApplicationScreenType ActionPlace { get; set; }
        public ApiMethod ActionType { get; set; }
    }
}
