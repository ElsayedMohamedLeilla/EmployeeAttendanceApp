namespace Dawem.Models.Response.Dawem.Core.VacationsTypes
{
    public class GetOvertimesTypeDropDownResponseDTO
    {
        public List<GetOvertimesTypeForDropDownResponseModelDTO> OvertimesTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
