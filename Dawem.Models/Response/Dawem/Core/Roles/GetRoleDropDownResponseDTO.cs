namespace Dawem.Models.Response.Dawem.Core.Roles
{
    public class GetRoleDropDownResponseDTO
    {
        public List<GetRoleForDropDownResponseModelDTO> Roles { get; set; }
        public int TotalCount { get; set; }
    }
}
