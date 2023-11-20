using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Models.Response.Core.JustificationsTypes;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IJustificationTypeBL
    {
        Task<int> Create(CreateJustificationsTypeDTO model);
        Task<bool> Update(UpdateJustificationsTypeDTO model);
        Task<GetJustificationsTypeInfoResponseDTO> GetInfo(int justificationsTypeId);
        Task<GetJustificationsTypeByIdResponseDTO> GetById(int justificationsTypeId);
        Task<GetJustificationsTypeResponseDTO> Get(GetJustificationsTypesCriteria model);
        Task<GetJustificationsTypeDropDownResponseDTO> GetForDropDown(GetJustificationsTypesCriteria model);
        Task<bool> Delete(int justificationsTypeId);
    }
}
