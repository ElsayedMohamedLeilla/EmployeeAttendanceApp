using Dawem.Models.Requests.Permissions;

namespace Dawem.Contract.BusinessValidation.Dawem.Requests
{
    public interface IRequestPermissionBLValidation
    {
        Task<int?> CreateValidation(CreateRequestPermissionModelDTO model);
        Task<int?> UpdateValidation(UpdateRequestPermissionModelDTO model);
    }
}
