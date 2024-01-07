namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetNonComplianceActionByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
