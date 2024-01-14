using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions
{
    public class BaseNonComplianceActionModel
    {
        public string Name { get; set; }
        public NonComplianceActionType Type { get; set; }
        public bool IsActive { get; set; }
        public string WarningMessage { get; set; }
    }
}
