namespace Dawem.Models.Response.Dawem.Core.VacationsTypes
{
    public class GetVacationsTypeDropDownResponseDTO
    {
        public List<GetVacationsTypeForDropDownResponseModelDTO> VacationsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
