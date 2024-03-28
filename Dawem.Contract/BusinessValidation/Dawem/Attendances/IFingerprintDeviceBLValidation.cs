using Dawem.Models.Dtos.Dawem.Attendances.FingerprintDevices;

namespace Dawem.Contract.BusinessValidation.Dawem.Attendances
{
    public interface IFingerprintDeviceBLValidation
    {
        Task<bool> CreateValidation(CreateFingerprintDeviceModel model);
        Task<bool> UpdateValidation(UpdateFingerprintDeviceModel model);
    }
}
