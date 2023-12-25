namespace Dawem.Models.Response.Core.PermissionsTypes
{
    public class GetPermissionsTypeByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
