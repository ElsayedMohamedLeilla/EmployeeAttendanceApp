namespace Dawem.Models.Response.Employees.Departments
{
    public class GetSubscriptionsResponse
    {
        public List<GetSubscriptionsResponseModel> Subscriptions { get; set; }
        public int TotalCount { get; set; }
    }
}
