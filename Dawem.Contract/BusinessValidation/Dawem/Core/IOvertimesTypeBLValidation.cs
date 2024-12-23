using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IOvertimesTypeBLValidation
    {
        Task<bool> CreateValidation(CreateOvertimeTypeDTO model);
        Task<bool> UpdateValidation(UpdateOvertimeTypeDTO model);
    }
}
