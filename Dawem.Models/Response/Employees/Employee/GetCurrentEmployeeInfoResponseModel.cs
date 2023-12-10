namespace Dawem.Models.Response.Employees.Employee
{
    public class GetCurrentEmployeeInfoResponseModel
    {
        public int EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string JobTitleName { get; set; }
        public string DapartmentName { get; set; }
        public string DirectManagerName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }        
        public string Address { get; set; }
        public string ProfileImagePath { get; set; }
    }
}
