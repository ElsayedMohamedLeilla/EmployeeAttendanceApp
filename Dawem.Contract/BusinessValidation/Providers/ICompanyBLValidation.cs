using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface ICompanyBLValidation
    {
        Task<bool> CreateValidation(CreateCompanyModel model);
        Task<bool> UpdateValidation(UpdateCompanyModel model);
    }
}
