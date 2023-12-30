using Dawem.Enums.Configration;

namespace Dawem.Models.Criteria.Others
{
    public class GetScreenPermissionLogsCriteria : BaseCriteria
    {
        public ApplicationScreenCode? ScreenCode { get; set; }
        public Enums.Configration.ApplicationAction? ActionType { get; set; }
    }
}
