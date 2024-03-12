namespace Dawem.Models.Response.Employees.Users
{
    public class GetUserInfoResponseModel
    {
        public string EmployeeName { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProfileImagePath { get; set; }
        public string ProfileImageName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
    }
}
