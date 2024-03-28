namespace Dawem.Models.Response.Dawem.Employees.Users
{
    public class GetUsersResponse
    {
        public List<GeUsersResponseModel> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
