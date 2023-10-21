using Dawem.Models.Dtos.Provider;
using Dawem.Models.Response.Employees;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IDepartmentBL
    {
        Task<int> Create(CreateDepartmentModel model);
        Task<bool> Update(UpdateDepartmentModel model);
        Task<GetDepartmentInfoResponseModel> GetInfo(int DepartmentId);
        Task<GetDepartmentByIdResponseModel> GetById(int DepartmentId);
        Task<GetDepartmentsResponse> Get(GetDepartmentsCriteria model);
        Task<GetDepartmentsForDropDownResponse> GetForDropDown(GetDepartmentsCriteria model);
        Task<bool> Delete(int DepartmentId);
    }
}
