using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dawem.Domain.Entities.Localization
{
    [Table(nameof(Translation) + "s")]
    public class Translation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }


        public int? AddUserId { get; set; }
        //[ForeignKey(nameof(AddUserId))]
        //public MyUser AddUser { get; set; }

        public int? ModifyUserId { get; set; }
        //[ForeignKey(nameof(ModifyUserId))]
        //public MyUser ModifyUser { get; set; }

        public int? CompanyId { get; set; }

        public int? BranchId { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }

        public bool Status { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }
        [StringLength(500)]
        public string? KeyWord { get; set; }

        public string? TransWords { get; set; }

        public string? Lang { get; set; }



    }
}
