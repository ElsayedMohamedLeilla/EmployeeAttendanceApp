using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Models.Response.Core.VacationsTypes;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IVacationTypeBL
    {
        Task<int> Create(CreateVacationsTypeDTO model);
        Task<bool> Update(UpdateVacationsTypeDTO model);
        Task<GetVacationsTypeInfoResponseDTO> GetInfo(int VacationsTypeId);
        Task<GetVacationsTypeByIdResponseDTO> GetById(int VacationsTypeId);
        Task<GetVacationsTypeResponseDTO> Get(GetVacationsTypeCriteria model);
        Task<GetVacationsTypeDropDownResponseDTO> GetForDropDown(GetVacationsTypeCriteria model);
        Task<bool> Delete(int VacationsTypeId);
    }
}
