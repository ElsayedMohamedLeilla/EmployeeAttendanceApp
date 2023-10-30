using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Models.Dtos.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IJustificationsTypeBLValidation
    {
        Task<bool> CreateValidation(CreateJustificationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateJustificationsTypeDTO model);
    }
}
