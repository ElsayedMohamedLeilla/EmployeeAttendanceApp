using Dawem.Models.Dtos.Employees.JobTitle;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IRequestTaskBLValidation
    {
        Task<bool> CreateValidation(CreateRequestTaskModelDTO model);
        Task<bool> UpdateValidation(UpdateRequestTaskModelDTO model);
    }
}
