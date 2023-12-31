using Dawem.Enums.Configration;

namespace Dawem.Models.Criteria.Others
{
    public class GetPermissionLogsCriteria : BaseCriteria
    {
        public ApplicationScreenCode? ScreenCode { get; set; }
        public Enums.Configration.ApplicationAction? ActionType { get; set; }
    }
}
