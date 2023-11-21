using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Provider;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(Group) + LeillaKeys.S)]
    public class Group : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int? GroupManagerId { get; set; }
        [ForeignKey(nameof(GroupManagerId))]
        public Employee GroupManager { get; set; }
        #endregion

        public int Code { get; set; }
        public string Name { get; set; }

        public List<GroupEmployee> GroupEmployees { get; set; }
       
        public List<GroupManagerDelegator> GroupManagerDelegators { get; set; }




    }
}
