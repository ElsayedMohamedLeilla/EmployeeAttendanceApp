namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultTasksTypes
{
    public class GetDefaultTasksTypeDropDownResponseDTO
    {
        public List<GetDefaultTasksTypeForDropDownResponseModelDTO> DefaultTasksTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
