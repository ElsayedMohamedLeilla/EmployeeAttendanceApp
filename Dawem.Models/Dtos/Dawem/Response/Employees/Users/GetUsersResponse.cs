namespace Dawem.Models.Response.Employees.Users
{
    public class GetUsersResponse
    {
        public List<GeUsersResponseModel> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
