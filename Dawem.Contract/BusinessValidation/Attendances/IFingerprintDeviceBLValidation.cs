using Dawem.Models.Dtos.Attendances.FingerprintDevices;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IFingerprintDeviceBLValidation
    {
        Task<bool> CreateValidation(CreateFingerprintDeviceModel model);
        Task<bool> UpdateValidation(UpdateFingerprintDeviceModel model);
    }
}
