using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Models.Response.Core.JustificationsTypes;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IJustificationsTypeBL
    {
        Task<int> Create(CreateJustificationsTypeDTO model);
        Task<bool> Update(UpdateJustificationsTypeDTO model);
        Task<GetJustificationsTypeInfoResponseDTO> GetInfo(int justificationsTypeId);
        Task<GetJustificationsTypeByIdResponseDTO> GetById(int justificationsTypeId);
        Task<GetJustificationsTypeResponseDTO> Get(GetJustificationsTypeCriteria model);
        Task<GetJustificationsTypeDropDownResponseDTO> GetForDropDown(GetJustificationsTypeCriteria model);
        Task<bool> Delete(int justificationsTypeId);
    }
}
