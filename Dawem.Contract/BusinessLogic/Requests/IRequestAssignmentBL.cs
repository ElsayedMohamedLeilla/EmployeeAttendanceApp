using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Assignments;
using Dawem.Models.Response.Attendances;
using Dawem.Models.Response.Requests.Assignments;
using Dawem.Models.Response.Requests.Vacations;

namespace Dawem.Contract.BusinessLogic.Requests
{
    public interface IRequestAssignmentBL
    {
        Task<int> Create(CreateRequestAssignmentModelDTO model);
        Task<bool> Update(UpdateRequestAssignmentModelDTO model);
        Task<GetRequestAssignmentInfoResponseModel> GetInfo(int requestId);
        Task<GetRequestAssignmentByIdResponseModel> GetById(int requestId);
        Task<GetRequestAssignmentsResponse> Get(GetRequestAssignmentsCriteria model);
        Task<List<EmployeeGetRequestAssignmentsResponseModel>> EmployeeGet(EmployeeGetRequestAssignmentsCriteria model);
        Task<GetRequestAssignmentsForDropDownResponse> GetForDropDown(GetRequestAssignmentsCriteria model);
        Task<bool> Accept(int requestId);
        Task<bool> Reject(RejectModelDTO rejectModelDTO);
        Task<bool> Delete(int requestId);
        Task<GetAssignmentsInformationsResponseDTO> GetAssignmentsInformations();
    }
}
