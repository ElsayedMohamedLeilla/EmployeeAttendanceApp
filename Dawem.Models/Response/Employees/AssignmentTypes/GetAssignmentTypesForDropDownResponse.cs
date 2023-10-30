namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetAssignmentTypesForDropDownResponse
    {
        public List<GetAssignmentTypesForDropDownResponseModel> AssignmentTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
