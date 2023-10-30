namespace Dawem.Models.Response.Employees.Employee
{
    public class GetEmployeeByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }
    }
}
