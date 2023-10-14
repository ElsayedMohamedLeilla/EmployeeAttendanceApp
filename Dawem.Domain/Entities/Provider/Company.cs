using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Dawem.Domain.Entities.Lookups;

namespace Dawem.Domain.Entities.Provider
{
    [Table("Companies")]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string? CompanyName { get; set; }
        public virtual List<Branch?>? Branches { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }
        public int? AddUserId { get; set; }
       

        public int? ModifyUserId { get; set; }
      

        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }


        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }
        public bool? IsSuspended { get; set; }

    }

}
