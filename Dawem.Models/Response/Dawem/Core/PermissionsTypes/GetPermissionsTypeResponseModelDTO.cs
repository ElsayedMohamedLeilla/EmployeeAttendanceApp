namespace Dawem.Models.Response.Dawem.Core.PermissionsTypes
{
    public class GetPermissionsTypeResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
