using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.Subscriptions.Plans
{
    public class GetPlanInfoResponseModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int MinNumberOfEmployees { get; set; }
        public int MaxNumberOfEmployees { get; set; }
        public decimal EmployeeCost { get; set; }
        public int SubscriptionsCount { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public List<NameTranslationGetInfoModel> NameTranslations { get; set; }
    }
}
