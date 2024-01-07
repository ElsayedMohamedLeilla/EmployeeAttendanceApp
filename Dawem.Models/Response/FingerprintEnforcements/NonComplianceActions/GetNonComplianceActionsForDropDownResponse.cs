namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetNonComplianceActionsForDropDownResponse
    {
        public List<GetNonComplianceActionsForDropDownResponseModel> NonComplianceActions { get; set; }
        public int TotalCount { get; set; }
    }
}
