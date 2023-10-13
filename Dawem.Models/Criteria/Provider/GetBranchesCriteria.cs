namespace Dawem.Models.Criteria.Provider
{
    public class GetBranchesCriteria : BaseCriteria
    {
        public bool GetWithOutStatusAndVisibleToGlamera { get; set; }
        public new int BranchId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? BranchName { get; set; }
        public bool? GetMainBranch { get; set; }
        public int CountryId { get; set; }
        public int UserId { get; set; }
    }
}
