namespace Dawem.Models.Criteria.Providers
{
    public class PackageCriteria : BaseCriteria
    {


        public int? Id { get; set; }

        public int? Level { get; set; }

        public bool IsActive { get; set; }


        public int?[] ScreenIds { get; set; }

    }
}
