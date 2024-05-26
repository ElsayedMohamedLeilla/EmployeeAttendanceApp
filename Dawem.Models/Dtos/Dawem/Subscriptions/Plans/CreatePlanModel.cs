using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Dtos.Dawem.Subscriptions.Plans
{
    public class CreatePlanModel : BaseCreateAndUpdateNameTranslation
    {
        public int MinNumberOfEmployees { get; set; }
        public int MaxNumberOfEmployees { get; set; }
        public decimal EmployeeCost { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }
        public bool AllScreensAvailable { get; set; }
        public List<int> ScreenIds { get; set; }
        public string Notes { get; set; }
    }
}
