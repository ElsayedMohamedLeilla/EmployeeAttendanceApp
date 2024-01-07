namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetFingerprintEnforcementsResponse
    {
        public List<GetFingerprintEnforcementsResponseModel> FingerprintEnforcements { get; set; }
        public int TotalCount { get; set; }
    }
}
