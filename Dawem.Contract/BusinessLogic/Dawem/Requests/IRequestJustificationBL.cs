using Dawem.Models.Dtos.Dawem.Requests;
using Dawem.Models.Dtos.Dawem.Requests.Justifications;
using Dawem.Models.Response.Requests.Justifications;

namespace Dawem.Contract.BusinessLogic.Dawem.Requests
{
    public interface IRequestJustificationBL
    {
        Task<int> Create(CreateRequestJustificationDTO model);
        Task<bool> Update(UpdateRequestJustificationDTO model);
        Task<GetRequestJustificationInfoResponseDTO> GetInfo(int requestId);
        Task<GetRequestJustificationByIdResponseDTO> GetById(int requestId);
        Task<GetRequestJustificationsResponseDTO> Get(GetRequestJustificationCriteria model);
        Task<EmployeeGetRequestJustificationsResponseDTO> EmployeeGet(EmployeeGetRequestJustificationCriteria model);
        Task<GetRequestJustificationsForDropDownResponseDTO> GetForDropDown(GetRequestJustificationCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
        Task<GetJustificationsInformationsResponseDTO> GetJustificationsInformations();
    }
}
