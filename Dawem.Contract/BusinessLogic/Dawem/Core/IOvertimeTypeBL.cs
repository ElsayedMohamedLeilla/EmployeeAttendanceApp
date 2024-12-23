using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Models.Response.Dawem.Core.VacationsTypes;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface IOvertimeTypeBL
    {
        Task<int> Create(CreateOvertimeTypeDTO model);
        Task<bool> Update(UpdateOvertimeTypeDTO model);
        Task<GetOvertimesTypeInfoResponseDTO> GetInfo(int overtimesTypeId);
        Task<GetOvertimesTypeByIdResponseDTO> GetById(int overtimesTypeId);
        Task<GetOvertimesTypeResponseDTO> Get(GetOvertimeTypesCriteria model);
        Task<GetOvertimesTypeDropDownResponseDTO> GetForDropDown(GetOvertimeTypesCriteria model);
        Task<bool> Delete(int overtimesTypeId);
        Task<GetOvertimesTypesInformationsResponseDTO> GetOvertimeTypesInformations();
    }
}
