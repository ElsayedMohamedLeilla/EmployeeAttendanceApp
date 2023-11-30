using Dawem.Enums.Generals;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public ApplicationType AddedApplicationType { get; set; }
        public ApplicationType? ModifiedApplicationType { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public string DisableReason { get; set; }
        public string Notes { get; set; }
        public void Delete()
        {
            IsDeleted = true;
            IsActive = false;
            DeletionDate = DateTime.UtcNow;
        }
        public void Disable(string disableReason = null)
        {
            IsActive = false;
            DisableReason = disableReason;
        }
        public void Enable()
        {
            IsActive = true;
            DisableReason = null;
        }

    }
}