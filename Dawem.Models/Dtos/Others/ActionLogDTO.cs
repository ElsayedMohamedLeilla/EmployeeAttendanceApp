using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Others
{
    public class ActionLogDTO
    {
        public int Id { get; set; }
        public ApplicationScreenType ActionPlace { get; set; }
        public ApiMethod ActionType { get; set; }
        public int BranchId { get; set; }
        public string? BranchGlobalName { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
