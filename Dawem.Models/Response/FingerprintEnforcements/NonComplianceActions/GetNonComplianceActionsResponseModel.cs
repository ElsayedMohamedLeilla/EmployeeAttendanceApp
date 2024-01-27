namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetNonComplianceActionsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
