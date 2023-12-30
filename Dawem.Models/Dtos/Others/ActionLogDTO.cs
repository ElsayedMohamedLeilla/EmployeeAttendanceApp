using Dawem.Enums.Configration;
using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Others
{
    public class ActionLogDTO
    {
        public int Id { get; set; }
        public ApplicationScreenCode ActionPlace { get; set; }
        public Enums.Configration.ApplicationAction ActionType { get; set; }
        public int BranchId { get; set; }
        public string? BranchGlobalName { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
