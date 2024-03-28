using Dawem.Models.Dtos.Dawem.Core.Zones;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IZoneBLValidation
    {
        Task<bool> CreateValidation(CreateZoneDTO model);
        Task<bool> UpdateValidation(UpdateZoneDTO model);
    }
}
