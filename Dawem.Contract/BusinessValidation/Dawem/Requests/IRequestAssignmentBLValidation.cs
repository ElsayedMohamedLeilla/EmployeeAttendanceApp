using Dawem.Models.Dtos.Dawem.Requests.Assignments;

namespace Dawem.Contract.BusinessValidation.Dawem.Requests
{
    public interface IRequestAssignmentBLValidation
    {
        Task<int?> CreateValidation(CreateRequestAssignmentModelDTO model);
        Task<int?> UpdateValidation(UpdateRequestAssignmentModelDTO model);
        Task<bool> GetEmployeeAssignmentsValidation(EmployeeGetRequestAssignmentsCriteria model);
    }
}
