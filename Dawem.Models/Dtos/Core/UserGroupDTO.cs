namespace Dawem.Models.Dtos.Core
{
    public class UserGroupDTO
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string? UserGlobalName { get; set; }
        public string? GroupGlobalName { get; set; }
    }
}
