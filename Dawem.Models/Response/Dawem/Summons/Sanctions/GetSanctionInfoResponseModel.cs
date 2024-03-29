namespace Dawem.Models.Response.Dawem.Summons.Sanctions
{
    public class GetSanctionInfoResponseModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string WarningMessage { get; set; }
        public bool IsActive { get; set; }
    }
}
