using Dawem.Models.Requests.Tasks;

namespace Dawem.Contract.BusinessValidation.Dawem.Requests
{
    public interface IRequestTaskBLValidation
    {
        Task<int?> CreateValidation(CreateRequestTaskModelDTO model);
        Task<int?> UpdateValidation(UpdateRequestTaskModelDTO model);
        Task<bool> GetEmployeeTasksValidation(EmployeeGetRequestTasksCriteria model);
    }
}
