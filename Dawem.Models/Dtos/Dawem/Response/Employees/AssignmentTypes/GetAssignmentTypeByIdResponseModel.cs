namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetAssignmentTypeByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
