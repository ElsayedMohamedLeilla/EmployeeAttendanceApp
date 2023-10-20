using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(Employee) + DawemKeys.S)]
    public class Employee : BaseEntity
    {
        public int Code { get; set; }
        public int DapartmentId { get; set; }
        [ForeignKey(nameof(DapartmentId))]
        public Department Dapartment { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
