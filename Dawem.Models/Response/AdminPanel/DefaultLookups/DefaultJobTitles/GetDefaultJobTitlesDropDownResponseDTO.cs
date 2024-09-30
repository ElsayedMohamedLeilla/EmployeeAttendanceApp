namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultJobTitles
{
    public class GetDefaultJobTitlesDropDownResponseDTO
    {
        public List<GetDefaultJobTitlesForDropDownResponseModelDTO> DefaultJobTitles { get; set; }
        public int TotalCount { get; set; }
    }
}
