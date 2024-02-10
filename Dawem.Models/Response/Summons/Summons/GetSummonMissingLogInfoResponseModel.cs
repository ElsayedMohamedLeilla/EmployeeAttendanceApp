namespace Dawem.Models.Response.Summons.Summons
{
    public class GetSummonMissingLogInfoResponseModel
    {
        public int Code { get; set; }
        public string EmployeeName { get; set; }
        public string SummonCode { get; set; }
        public DateTime SummonDate { get; set; }
        public string SummonForTypeName { get; set; }
        public string SummonAllowedTimeName { get; set; }
        public bool DoneNotify { get; set; }
    }
}
