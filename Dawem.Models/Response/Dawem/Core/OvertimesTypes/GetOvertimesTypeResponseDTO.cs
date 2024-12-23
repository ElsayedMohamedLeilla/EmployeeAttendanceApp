namespace Dawem.Models.Response.Dawem.Core.VacationsTypes
{
    public class GetOvertimesTypeResponseDTO
    {
        public List<GetOvertimesTypeResponseModelDTO> OvertimesTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
