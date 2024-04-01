namespace Dawem.Models.Response.Dawem.Employees.Users
{
    public class AdminPanelGetUserInfoResponseModel
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public string Email { get; set; }
        public string ProfileImagePath { get; set; }
        public string ProfileImageName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public List<string> Responsibilities { get; set; }
    }
}
