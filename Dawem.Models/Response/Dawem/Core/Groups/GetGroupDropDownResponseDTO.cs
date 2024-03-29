namespace Dawem.Models.Response.Dawem.Core.Groups
{
    public class GetGroupDropDownResponseDTO
    {
        public List<GetGroupForDropDownResponseModelDTO> Groups { get; set; }
        public int TotalCount { get; set; }
    }
}
