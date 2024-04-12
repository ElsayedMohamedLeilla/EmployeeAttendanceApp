namespace Dawem.Models.Dtos.Dawem.Providers.Companies
{
    public class AdminPanelUpdateCompanyModel : BaseCompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfEmployees { get; set; }
    }
}
