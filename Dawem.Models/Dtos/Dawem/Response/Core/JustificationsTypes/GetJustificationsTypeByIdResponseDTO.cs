namespace Dawem.Models.Response.Core.JustificationsTypes
{
    public class GetJustificationsTypeByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
