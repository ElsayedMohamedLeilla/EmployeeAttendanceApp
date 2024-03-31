namespace Dawem.Models.Response.Dawem.Core.Responsibilities
{
    public class GetResponsibilitiesResponse
    {
        public List<GetResponsibilitiesResponseModel> Responsibilities { get; set; }
        public int TotalCount { get; set; }
    }
}
