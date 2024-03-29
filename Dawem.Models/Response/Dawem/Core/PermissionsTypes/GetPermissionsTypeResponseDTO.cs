namespace Dawem.Models.Response.Dawem.Core.PermissionsTypes
{
    public class GetPermissionsTypeResponseDTO
    {
        public List<GetPermissionsTypeResponseModelDTO> PermissionsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
