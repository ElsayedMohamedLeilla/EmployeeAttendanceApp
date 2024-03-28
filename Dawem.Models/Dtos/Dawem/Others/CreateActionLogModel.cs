using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class CreateActionLogModel
    {
        public ApplicationScreenCode ActionPlace { get; set; }
        public ApplicationAction ActionType { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
