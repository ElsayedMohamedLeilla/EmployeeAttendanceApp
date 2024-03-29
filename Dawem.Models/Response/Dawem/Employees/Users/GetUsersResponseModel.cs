namespace Dawem.Models.Response.Dawem.Employees.Users
{
    public class GeUsersResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

    }
}
