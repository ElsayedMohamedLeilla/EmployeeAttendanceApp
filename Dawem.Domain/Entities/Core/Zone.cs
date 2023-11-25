using Dawem.Domain.Entities.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Domain.Entities.Core
{
    public class Zone : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion


        public int Code { get; set; }
        public string Name { get; set; }
        public  decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Radius { get; set; }
        
    }
}
