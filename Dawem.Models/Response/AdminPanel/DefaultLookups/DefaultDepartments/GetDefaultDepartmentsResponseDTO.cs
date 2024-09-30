namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultDepartments
{
    public class GetDefaultDepartmentsResponseDTO
    {
        public List<GetDefaultDepartmentsResponseModelDTO> DefaultDepartments { get; set; }
        public int TotalCount { get; set; }
    }
}
