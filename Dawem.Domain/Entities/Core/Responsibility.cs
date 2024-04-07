using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(LeillaKeys.Responsibilities)]
    public class Responsibility : BaseEntity
    {
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public AuthenticationType Type { get; set; }
        public List<UserResponsibility> ResponsibilityUsers { get; set; }
    }
}
