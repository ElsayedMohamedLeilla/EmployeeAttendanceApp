namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultDepartments
{
    public class GetDefaultDepartmentsDropDownResponseDTO
    {
        public List<GetDefaultDepartmentsForDropDownResponseModelDTO> DefaultDepartments { get; set; }
        public int TotalCount { get; set; }
    }
}
