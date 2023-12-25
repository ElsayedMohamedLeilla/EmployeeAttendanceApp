using Dawem.Models.Dtos.Requests.Tasks;

namespace Dawem.Contract.BusinessValidation.Requests
{
    public interface IRequestTaskBLValidation
    {
        Task<int?> CreateValidation(CreateRequestTaskModelDTO model);
        Task<int?> UpdateValidation(UpdateRequestTaskModelDTO model);
        Task<bool> GetEmployeeTasksValidation(EmployeeGetRequestTasksCriteria model);
    }
}
