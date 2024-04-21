using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface IEmployeeOTPBLValidation
    {
        Task<int> PreSignUpValidation(PreSignUpDTO model);
        Task<bool> CreateValidation(CreateEmployeeOTPDTO model);

    }
}
