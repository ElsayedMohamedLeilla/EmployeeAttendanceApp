using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Excel;
using Dawem.Models.Response.Employees.Employees;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IEmployeeBL
    {
        Task<int> Create(CreateEmployeeModel model);
        Task<bool> Update(UpdateEmployeeModel model);
        Task<GetEmployeeInfoResponseModel> GetInfo(int employeeId);
        Task<GetCurrentEmployeeInfoResponseModel> GetCurrentEmployeeInfo();
        Task<GetEmployeeByIdResponseModel> GetById(int employeeId);
        Task<GetEmployeesResponse> Get(GetEmployeesCriteria model);
        Task<GetEmployeesForDropDownResponse> GetForDropDown(GetEmployeesCriteria model);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int employeeId);
        Task<bool> Delete(int employeeId);
        Task<GetEmployeesInformationsResponseDTO> GetEmployeesInformations();
        public  Task<MemoryStream> ExportDraft();
        public Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile);

        Task<bool> UpdateSpecificDataForEmployee(UpdateSpecificModelDTO model);

    }
}
