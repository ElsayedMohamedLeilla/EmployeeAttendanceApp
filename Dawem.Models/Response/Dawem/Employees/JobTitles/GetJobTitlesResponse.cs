namespace Dawem.Models.Response.Dawem.Employees.JobTitles
{
    public class GetJobTitlesResponse
    {
        public List<GetJobTitlesResponseModel> JobTitles { get; set; }
        public int TotalCount { get; set; }
    }
}
