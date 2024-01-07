namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetNonComplianceActionsResponse
    {
        public List<GetNonComplianceActionsResponseModel> NonComplianceActions { get; set; }
        public int TotalCount { get; set; }
    }
}
