namespace Dawem.Models.Response.Dawem.Core.Groups
{
    public class GetGroupInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<string> Employees { get; set; }
        public string ManagerName { get; set; }
        public List<string> ManagerDelegators { get; set; }
        public List<string> Zones { get; set; }
    }
}
