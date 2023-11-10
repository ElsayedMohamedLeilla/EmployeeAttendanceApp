namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupDropDownResponseDTO
    {
        public List<GetGroupForDropDownResponseModelDTO> Groups { get; set; }
        public int TotalCount { get; set; }
    }
}
