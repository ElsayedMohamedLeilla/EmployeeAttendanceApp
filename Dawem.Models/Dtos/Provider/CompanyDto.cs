using Dawem.Models.Dtos.Lookups;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Models.Dtos.Provider
{
    public class CompanyDto
    {

        public int Id { get; set; }

        public bool IsActive { get; set; }
        public int? CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]

        public CountryDTO Country { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameCulture { get; set; }

        public virtual List<BranchDTO> Branches { get; set; }


        public bool? IsSuspended { get; set; }
    }
}
