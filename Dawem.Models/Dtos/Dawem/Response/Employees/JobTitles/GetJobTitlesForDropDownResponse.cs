namespace Dawem.Models.Response.Employees.JobTitles
{
    public class GetJobTitlesForDropDownResponse
    {
        public List<GetJobTitlesForDropDownResponseModel> JobTitles { get; set; }
        public int TotalCount { get; set; }
    }
}
