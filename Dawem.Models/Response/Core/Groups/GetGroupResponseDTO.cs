namespace Dawem.Models.Response.Core.Groups
{
    public class GetGroupResponseDTO
    {
        public List<GetGroupResponseModelDTO> Groups { get; set; }
        public int TotalCount { get; set; }
    }
}
