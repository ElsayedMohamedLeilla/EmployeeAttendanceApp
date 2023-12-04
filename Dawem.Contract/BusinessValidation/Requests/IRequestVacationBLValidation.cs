using Dawem.Models.Dtos.Requests.Vacations;

namespace Dawem.Contract.BusinessValidation.Requests
{
    public interface IRequestVacationBLValidation
    {
        Task<int?> CreateValidation(CreateRequestVacationDTO model);
        Task<int?> UpdateValidation(UpdateRequestVacationDTO model);
    }
}
