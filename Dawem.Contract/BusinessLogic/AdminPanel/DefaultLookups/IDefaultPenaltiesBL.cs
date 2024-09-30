using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultPenalties;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultPenaltiesBL
    {
        Task<int> Create(CreateDefaultPenaltiesDTO model);
        Task<bool> Update(UpdateDefaultPenaltiesDTO model);
        Task<GetDefaultPenaltiesInfoResponseDTO> GetInfo(int DefaultPenaltiesId);
        Task<GetDefaultPenaltiesByIdResponseDTO> GetById(int DefaultPenaltiesId);
        Task<GetDefaultPenaltiesResponseDTO> Get(GetDefaultPenaltiesCriteria model);
        Task<GetDefaultPenaltiesDropDownResponseDTO> GetForDropDown(GetDefaultPenaltiesCriteria model);
        Task<bool> Delete(int DefaultPenaltiesId);
        public Task<bool> Enable(int GroupId);
        public Task<bool> Disable(DisableModelDTO model);


    }
}
