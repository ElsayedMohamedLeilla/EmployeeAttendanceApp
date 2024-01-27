using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetNonComplianceActionInfoResponseModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public NonComplianceActionType Type { get; set; }
        public string TypeName { get; set; }
        public string WarningMessage { get; set; }
        public bool IsActive { get; set; }
    }
}
