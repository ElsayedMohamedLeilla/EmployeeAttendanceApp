using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class CreateActionLogModel
    {
        public DawemAdminApplicationScreenCode ActionPlace { get; set; }
        public ApplicationActionCode ActionType { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
