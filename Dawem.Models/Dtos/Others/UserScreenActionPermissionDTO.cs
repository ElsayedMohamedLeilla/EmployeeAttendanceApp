using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Others
{
    public class UserScreenActionPermissionDTO
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
        public ApplicationScreenCode ActionPlace { get; set; }
        public ApplicationAction ActionType { get; set; }
    }
}
