namespace Dawem.Models.Response.Employees
{
    public class GetEmployeesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string DapartmentName { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }
    }
}
