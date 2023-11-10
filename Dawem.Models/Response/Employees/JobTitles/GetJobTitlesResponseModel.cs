namespace Dawem.Models.Response.Employees.JobTitles
{
    public class GetJobTitlesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
