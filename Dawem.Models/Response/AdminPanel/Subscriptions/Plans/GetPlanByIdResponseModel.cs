using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetPlanByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int MinNumberOfEmployees { get; set; }
        public int MaxNumberOfEmployees { get; set; }
        public decimal EmployeeCost { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }
        public bool AllScreensAvailable { get; set; }
        public List<int> ScreenIds { get; set; }
        public string Notes { get; set; }
        public List<NameTranslationModel> NameTranslations { get; set; }

    }
}
