namespace Dawem.Models.Response.Core.PermissionsTypes
{
    public class GetPermissionsTypeResponseDTO
    {
        public List<GetPermissionsTypeResponseModelDTO> PermissionsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
