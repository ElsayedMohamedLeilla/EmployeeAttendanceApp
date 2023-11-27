using Dawem.Models.Dtos.Core.Zones;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IZoneBLValidation
    {
        Task<bool> CreateValidation(CreateZoneDTO model);
        Task<bool> UpdateValidation(UpdateZoneDTO model);
    }
}
