namespace Dawem.Models.Dtos.Subscriptions.Plans
{
    public class CreatePlanModel
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int MinNumberOfEmployees { get; set; }
        public int MaxNumberOfEmployees { get; set; }
        public decimal EmployeeCost { get; set; }
        public int GracePeriodPercentage { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
