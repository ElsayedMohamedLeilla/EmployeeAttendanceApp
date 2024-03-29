using Dawem.Models.Requests.Vacations;

namespace Dawem.Contract.BusinessValidation.Dawem.Requests
{
    public interface IRequestVacationBLValidation
    {
        Task<int?> CreateValidation(CreateRequestVacationDTO model);
        Task<int?> UpdateValidation(UpdateRequestVacationDTO model);
    }
}
