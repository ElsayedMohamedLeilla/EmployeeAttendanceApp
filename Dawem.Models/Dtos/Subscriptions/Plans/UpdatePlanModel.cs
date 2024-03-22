using Dawem.Models.Dtos.Shared;

namespace Dawem.Models.Dtos.Subscriptions.Plans
{
    public class UpdatePlanModel
    {
        public int Id { get; set; }
        public int MinNumberOfEmployees { get; set; }
        public int MaxNumberOfEmployees { get; set; }
        public decimal EmployeeCost { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public List<NameTranslationModel> NameTranslations { get; set; }
    }
}
