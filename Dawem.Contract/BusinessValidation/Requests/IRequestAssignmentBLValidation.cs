using Dawem.Models.Dtos.Requests.Assignments;

namespace Dawem.Contract.BusinessValidation.Requests
{
    public interface IRequestAssignmentBLValidation
    {
        Task<int?> CreateValidation(CreateRequestAssignmentModelDTO model);
        Task<int?> UpdateValidation(UpdateRequestAssignmentModelDTO model);
    }
}
