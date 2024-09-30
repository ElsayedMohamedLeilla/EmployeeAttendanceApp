namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultPenalties
{
    public class GetDefaultPenaltiesDropDownResponseDTO
    {
        public List<GetDefaultPenaltiesForDropDownResponseModelDTO> DefaultPenalties { get; set; }
        public int TotalCount { get; set; }
    }
}
