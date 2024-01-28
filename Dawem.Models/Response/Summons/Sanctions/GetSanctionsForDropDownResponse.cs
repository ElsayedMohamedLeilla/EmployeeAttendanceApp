namespace Dawem.Models.Response.Summons.Sanctions
{
    public class GetSanctionsForDropDownResponse
    {
        public List<GetSanctionsForDropDownResponseModel> Sanctions { get; set; }
        public int TotalCount { get; set; }
    }
}
