namespace Dawem.Models.Response.Employees.User
{
    public class GetUsersForDropDownResponse
    {
        public List<GetUsersForDropDownResponseModel> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
