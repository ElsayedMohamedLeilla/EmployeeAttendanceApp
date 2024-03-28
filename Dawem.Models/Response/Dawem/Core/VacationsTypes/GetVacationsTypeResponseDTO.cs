namespace Dawem.Models.Response.Dawem.Core.VacationsTypes
{
    public class GetVacationsTypeResponseDTO
    {
        public List<GetVacationsTypeResponseModelDTO> VacationsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
