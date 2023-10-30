using Dawem.Models.Response.Employees.User;
using Dawem.Models.ResponseModels;

namespace Dawem.Models.Response.Employees.Employee
{
    public class GetUsersResponse
    {
        public List<GeUsersResponseModel> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
