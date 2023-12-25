using Dawem.Models.Dtos.Core.VacationsTypes;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IVacationTypeBLValidation
    {
        Task<bool> CreateValidation(CreateVacationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateVacationsTypeDTO model);
    }
}
