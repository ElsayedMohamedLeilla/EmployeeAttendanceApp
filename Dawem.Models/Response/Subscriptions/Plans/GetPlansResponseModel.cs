namespace Dawem.Models.Response.Employees.Departments
{
    public class GetPlansResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public decimal EmployeeCost { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }

    }
}
