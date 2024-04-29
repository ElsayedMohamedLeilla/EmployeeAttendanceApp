namespace Dawem.Models.Dtos.Dawem.Employees.Users
{
    public class CreateUserModel
    {
        public int EmployeeId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
        public List<int> Responsibilities { get; set; }
    }
}
