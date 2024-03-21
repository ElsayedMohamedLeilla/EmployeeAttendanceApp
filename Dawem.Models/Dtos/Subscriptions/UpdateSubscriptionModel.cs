using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Employees.Departments
{
    public class UpdateSubscriptionModel
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int CompanyId { get; set; }
        public int DurationInDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public int RenewalCount { get; set; }
        public string FollowUpEmail { get; set; }
        public string Notes { get; set; }
    }
}
