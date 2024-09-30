using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultJobTitles;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultJobTitlesBL
    {
        Task<int> Create(CreateDefaultJobTitlesDTO model);
        Task<bool> Update(UpdateDefaultJobTitlesDTO model);
        Task<GetDefaultJobTitlesInfoResponseDTO> GetInfo(int DefaultJobTitlesId);
        Task<GetDefaultJobTitlesByIdResponseDTO> GetById(int DefaultJobTitlesId);
        Task<GetDefaultJobTitlesResponseDTO> Get(GetDefaultJobTitlesCriteria model);
        Task<GetDefaultJobTitlesDropDownResponseDTO> GetForDropDown(GetDefaultJobTitlesCriteria model);
        Task<bool> Delete(int DefaultJobTitlesId);
        public Task<bool> Enable(int GroupId);
        public Task<bool> Disable(DisableModelDTO model);


    }
}
