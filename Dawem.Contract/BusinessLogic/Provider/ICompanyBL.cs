using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Employees.Employees;

namespace Dawem.Contract.BusinessLogic.Companies
{
    public interface ICompanyBL
    {
        Task<int> Create(CreateCompanyModel model);
        Task<bool> Update(UpdateCompanyModel model);
        Task<GetCompanyInfoResponseModel> GetInfo(int companyId);
        Task<GetCompanyByIdResponseModel> GetById(int companyId);
        Task<GetCompaniesResponse> Get(GetCompaniesCriteria model);
        Task<GetCompaniesForDropDownResponse> GetForDropDown(GetCompaniesCriteria model);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Enable(int companyId);
        Task<bool> Delete(int companyId);
        Task<GetCompaniesInformationsResponseDTO> GetCompaniesInformations();
    }
}
