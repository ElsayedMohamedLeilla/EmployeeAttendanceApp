namespace Dawem.Models.Response.Dawem.Employees.Users
{
    public class AdminPanelGetUserByIdResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImagePath { get; set; }
        public List<int> Responsibilities { get; set; }
    }
}
