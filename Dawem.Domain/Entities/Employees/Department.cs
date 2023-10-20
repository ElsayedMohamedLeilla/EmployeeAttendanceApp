using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(Department) + DawemKeys.S)]
    public class Department : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
