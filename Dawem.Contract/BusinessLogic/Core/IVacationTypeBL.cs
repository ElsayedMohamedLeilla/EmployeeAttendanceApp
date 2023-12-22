using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Models.Response.Core.VacationsTypes;
using Dawem.Models.Response.Requests.Vacations;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IVacationTypeBL
    {
        Task<int> Create(CreateVacationsTypeDTO model);
        Task<bool> Update(UpdateVacationsTypeDTO model);
        Task<GetVacationsTypeInfoResponseDTO> GetInfo(int VacationsTypeId);
        Task<GetVacationsTypeByIdResponseDTO> GetById(int VacationsTypeId);
        Task<GetVacationsTypeResponseDTO> Get(GetVacationsTypesCriteria model);
        Task<GetVacationsTypeDropDownResponseDTO> GetForDropDown(GetVacationsTypesCriteria model);
        Task<bool> Delete(int VacationsTypeId);
        Task<GetVacationsTypesInformationsResponseDTO> GetVacationTypesInformations();
    }
}
