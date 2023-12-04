using Dawem.Models.Dtos.Requests.Justifications;

namespace Dawem.Contract.BusinessValidation.Requests
{
    public interface IRequestJustificationBLValidation
    {
        Task<int?> CreateValidation(CreateRequestJustificationDTO model);
        Task<int?> UpdateValidation(UpdateRequestJustificationDTO model);
    }
}
