using Dawem.Contract.Repository.Manager;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Translations;

namespace Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups
{
    public interface IDefaultVacationTypeBL
    {
        Task<int> Create(CreateDefaultVacationsTypeDTO model);
        Task<bool> Update(UpdateDefaultVacationsTypeDTO model);
        Task<GetDefaultVacationsTypeInfoResponseDTO> GetInfo(int DefaultVacationsTypeId);
        Task<GetDefaultVacationsTypeByIdResponseDTO> GetById(int DefaultVacationsTypeId);
        Task<GetDefaultVacationsTypeResponseDTO> Get(GetDefaultVacationTypeCriteria model);
        Task<GetDefaultVacationsTypeDropDownResponseDTO> GetForDropDown(GetDefaultVacationTypeCriteria model);
        Task<bool> Delete(int DefaultVacationsTypeId);

        public  Task<bool> Enable(int GroupId);

        public  Task<bool> Disable(DisableModelDTO model);
       

    }
}
