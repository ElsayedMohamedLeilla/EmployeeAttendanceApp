namespace Dawem.Models.Response.Subscriptions.Plans
{
    public class GetPlansResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal EmployeeCost { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }
    }
}
