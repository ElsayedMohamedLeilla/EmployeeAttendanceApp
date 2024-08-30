using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultShiftsTypes;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultShiftTypeBL
    {
        Task<int> Create(CreateDefaultShiftsTypeDTO model);
        Task<bool> Update(UpdateDefaultShiftsTypeDTO model);
        Task<GetDefaultShiftsTypeInfoResponseDTO> GetInfo(int DefaultShiftTypeId);
        Task<GetDefaultShiftsTypeByIdResponseDTO> GetById(int DefaultShiftTypeId);
        Task<GetDefaultShiftsTypeResponseDTO> Get(GetDefaultShiftTypeCriteria model);
        Task<GetDefaultShiftsTypeDropDownResponseDTO> GetForDropDown(GetDefaultShiftTypeCriteria model);
        Task<bool> Delete(int DefaultShiftTypeId);

        public Task<bool> Enable(int GroupId);

        public Task<bool> Disable(DisableModelDTO model);


    }
}
