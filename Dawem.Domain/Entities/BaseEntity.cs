using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dawem.Domain.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }


        public int? AddUserId { get; set; }


        public int? ModifyUserId { get; set; }


        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool Status { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }

    }
}