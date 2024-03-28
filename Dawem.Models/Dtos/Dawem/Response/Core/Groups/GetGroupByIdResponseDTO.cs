namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> EmployeeIds { get; set; }
        public List<int> ManagerDelegatorIds { get; set; }
        public int? ManagerId { get; set; }

        public List<int> ZoneIds { get; set; }





    }
}
