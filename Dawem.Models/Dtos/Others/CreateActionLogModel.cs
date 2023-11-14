using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Others
{
    public class CreateActionLogModel
    {
        public ApplicationScreenType ActionPlace { get; set; }
        public ApiMethod ActionType { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
