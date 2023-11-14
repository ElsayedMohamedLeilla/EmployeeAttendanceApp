using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Shared
{
    public class ValidationItems
    {

        public string Item { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
        public int? Id { get; set; }
        public int? BranchId { get; set; }
        public int? CompanyId { get; set; }

        public int? Code { get; set; }

        public ValidationMode validationMode { get; set; }
    }
}
