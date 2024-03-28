namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetAssignmentTypesResponse
    {
        public List<GetAssignmentTypesResponseModel> AssignmentTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
