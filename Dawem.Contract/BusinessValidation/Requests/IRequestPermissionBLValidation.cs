using Dawem.Models.Dtos.Requests.Tasks;

namespace Dawem.Contract.BusinessValidation.Requests
{
    public interface IRequestPermissionBLValidation
    {
        Task<int?> CreateValidation(CreateRequestPermissionModelDTO model);
        Task<int?> UpdateValidation(UpdateRequestPermissionModelDTO model);
    }
}
