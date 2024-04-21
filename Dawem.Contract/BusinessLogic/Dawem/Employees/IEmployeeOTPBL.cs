using Dawem.Models.Dtos.Dawem.Employees.Users;

namespace Dawem.Contract.BusinessLogic.Dawem.Employees
{
    public interface IEmployeeOTPBL
    {

        Task<int> PreSignUp(PreSignUpDTO model);
        Task<bool> SendOTPByEmail(SendOTPByEmailDTO model);
        Task<string> GetOTPByEmployeeId(int employeeId);
        Task<bool> Delete(int employeeId);
    }
}
