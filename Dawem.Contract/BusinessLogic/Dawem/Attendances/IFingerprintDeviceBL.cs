using Dawem.Models.Dtos.Dawem.Attendances.FingerprintDevices;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.Attendances.FingerprintDevices;

namespace Dawem.Contract.BusinessLogic.Dawem.Attendances
{
    public interface IFingerprintDeviceBL
    {
        Task<int> Create(CreateFingerprintDeviceModel model);
        Task<bool> Update(UpdateFingerprintDeviceModel model);
        Task<GetFingerprintDeviceInfoResponseModel> GetInfo(int fingerprintDeviceId);
        Task<GetFingerprintDeviceByIdResponseModel> GetById(int fingerprintDeviceId);
        Task<GetFingerprintDevicesResponse> Get(GetFingerprintDevicesCriteria model);
        Task<GetFingerprintDevicesForDropDownResponse> GetForDropDown(GetFingerprintDevicesCriteria model);
        Task<bool> Delete(int fingerprintDeviceId);
        Task<bool> Enable(int fingerprintDeviceId);
        Task<bool> Disable(DisableModelDTO model);
        Task<GetFingerprintDevicesInformationsResponseDTO> GetFingerprintDevicesInformations();
    }
}
