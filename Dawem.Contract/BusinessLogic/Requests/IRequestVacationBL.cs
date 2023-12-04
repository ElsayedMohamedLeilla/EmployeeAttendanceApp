using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Vacations;
using Dawem.Models.Response.Requests.Vacations;

namespace Dawem.Contract.BusinessLogic.Requests
{
    public interface IRequestVacationBL
    {
        Task<int> Create(CreateRequestVacationDTO model);
        Task<bool> Update(UpdateRequestVacationDTO model);
        Task<GetRequestVacationInfoResponseDTO> GetInfo(int requestId);
        Task<GetRequestVacationByIdResponseDTO> GetById(int requestId);
        Task<GetRequestVacationsResponseDTO> Get(GetRequestVacationCriteria model);
        Task<GetRequestVacationsForDropDownResponseDTO> GetForDropDown(GetRequestVacationCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
    }
}
