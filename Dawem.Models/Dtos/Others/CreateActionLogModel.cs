using Dawem.Enums.Configration;
using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Others
{
    public class CreateActionLogModel
    {
        public ApplicationScreenCode ActionPlace { get; set; }
        public Enums.Configration.ApplicationAction ActionType { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
