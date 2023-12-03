using Dawem.Models.Dtos.Employees.JobTitle;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IRequestTaskBLValidation
    {
        Task<int?> CreateValidation(CreateRequestTaskModelDTO model);
        Task<int?> UpdateValidation(UpdateRequestTaskModelDTO model);
    }
}
