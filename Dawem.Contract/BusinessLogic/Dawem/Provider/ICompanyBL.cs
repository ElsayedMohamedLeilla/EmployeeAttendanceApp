using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Models.Response.Dawem.Providers.Companies;

namespace Dawem.Contract.BusinessLogic.Dawem.Provider
{
    public interface ICompanyBL
    {
        Task<int> Create(CreateCompanyModel model);
        Task<bool> Update(UpdateCompanyModel model);
        Task<bool> Update(AdminPanelUpdateCompanyModel model);
        Task<GetCompanyInfoResponseModel> GetInfo(int companyId);
        Task<GetCompanyByIdResponseModel> GetById(int companyId);
        Task<AdminPanelGetCompanyByIdResponseModel> GetById();
        Task<GetCompaniesResponse> Get(GetCompaniesCriteria model);
        Task<GetCompaniesForDropDownResponse> GetForDropDown(GetCompaniesCriteria model);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int companyId);
        Task<bool> Delete(int companyId);
        Task<GetCompaniesInformationsResponseDTO> GetCompaniesInformations();
    }
}
