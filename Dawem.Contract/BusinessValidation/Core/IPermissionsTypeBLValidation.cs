using Dawem.Models.Dtos.Core.PermissionsTypes;
using Dawem.Models.Dtos.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IPermissionsTypeBLValidation
    {
        Task<bool> CreateValidation(CreatePermissionsTypeDTO model);
        Task<bool> UpdateValidation(UpdatePermissionsTypeDTO model);
    }
}
