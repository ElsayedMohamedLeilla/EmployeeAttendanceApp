using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IVacationTypeBLValidation
    {
        Task<bool> CreateValidation(CreateVacationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateVacationsTypeDTO model);
    }
}
