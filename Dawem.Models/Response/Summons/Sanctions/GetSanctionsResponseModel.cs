namespace Dawem.Models.Response.Summons.Sanctions
{
    public class GetSanctionsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
