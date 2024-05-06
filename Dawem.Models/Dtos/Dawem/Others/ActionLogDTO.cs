using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class ActionLogDTO
    {
        public int Id { get; set; }
        public DawemAdminApplicationScreenCode ActionPlace { get; set; }
        public DawemAdminApplicationAction ActionType { get; set; }
        public int BranchId { get; set; }
        public string BranchGlobalName { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
