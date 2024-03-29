using Dawem.Models.Dtos.Dawem.Employees.Departments;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.Dawem.Employees.Departments;

namespace Dawem.Contract.BusinessLogic.Dawem.Employees.Department
{
    public interface IDepartmentBL
    {
        Task<int> Create(CreateDepartmentModel model);
        Task<bool> Update(UpdateDepartmentModel model);
        Task<GetDepartmentInfoResponseModel> GetInfo(int departmentId3);
        Task<GetDepartmentByIdResponseModel> GetById(int departmentId);
        Task<GetDepartmentsResponse> Get(GetDepartmentsCriteria model);
        Task<GetDepartmentsForDropDownResponse> GetForDropDown(GetDepartmentsCriteria model);
        Task<GetDepartmentsForTreeResponse> GetForTree(GetDepartmentsForTreeCriteria model);
        Task<bool> Delete(int departmentId);
        Task<bool> Enable(int departmentId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetDepartmentsInformationsResponseDTO> GetDepartmentsInformations();

        public Task<MemoryStream> ExportDraft();
        public Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile);
    }
}
