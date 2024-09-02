using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultJustificationsTypes;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultJustificationTypeBL
    {
        Task<int> Create(CreateDefaultJustificationsTypeDTO model);
        Task<bool> Update(UpdateDefaultJustificationsTypeDTO model);
        Task<GetDefaultJustificationsTypeInfoResponseDTO> GetInfo(int DefaultJustificationsTypeId);
        Task<GetDefaultJustificationsTypeByIdResponseDTO> GetById(int DefaultJustificationsTypeId);
        Task<GetDefaultJustificationsTypeResponseDTO> Get(GetDefaultJustificationTypeCriteria model);
        Task<GetDefaultJustificationsTypeDropDownResponseDTO> GetForDropDown(GetDefaultJustificationTypeCriteria model);
        Task<bool> Delete(int DefaultJustificationsTypeId);

        public Task<bool> Enable(int justificationTypeId);

        public Task<bool> Disable(DisableModelDTO model);


    }
}
