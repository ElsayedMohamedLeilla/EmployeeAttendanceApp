using Dawem.Models.Requests.Justifications;

namespace Dawem.Contract.BusinessValidation.Dawem.Requests
{
    public interface IRequestOvertimeBLValidation
    {
        Task<int?> CreateValidation(CreateRequestOvertimeDTO model);
        Task<int?> UpdateValidation(UpdateRequestOvertimeDTO model);
    }
}
