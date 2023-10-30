using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Models.Dtos.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IVacationsTypeBLValidation
    {
        Task<bool> CreateValidation(CreateVacationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateVacationsTypeDTO model);
    }
}
