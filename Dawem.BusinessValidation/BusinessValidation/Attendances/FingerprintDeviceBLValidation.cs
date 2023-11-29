using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances.FingerprintDevices;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Employees
{

    public class FingerprintDeviceBLValidation : IFingerprintDeviceBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public FingerprintDeviceBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateFingerprintDeviceModel model)
        {
            var checkFingerprintDeviceDuplicate = await repositoryManager
                .FingerprintDeviceRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkFingerprintDeviceDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceNameIsDuplicated);
            }

            var checkFingerprintDeviceSerialNumberDuplicate = await repositoryManager
                .FingerprintDeviceRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.SerialNumber == model.SerialNumber).AnyAsync();
            if (checkFingerprintDeviceSerialNumberDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceSerialNumberIsDuplicated);
            }

            var checkFingerprintDeviceIpAddressDuplicate = await repositoryManager
                .FingerprintDeviceRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.IpAddress == model.IpAddress).AnyAsync();
            if (checkFingerprintDeviceIpAddressDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceIpAddressIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateFingerprintDeviceModel model)
        {
            var checkFingerprintDeviceDuplicate = await repositoryManager
                .FingerprintDeviceRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkFingerprintDeviceDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceNameIsDuplicated);
            }

            var checkFingerprintDeviceSerialNumberDuplicate = await repositoryManager
               .FingerprintDeviceRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.SerialNumber == model.SerialNumber && c.Id != model.Id).AnyAsync();
            if (checkFingerprintDeviceSerialNumberDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceSerialNumberIsDuplicated);
            }

            var checkFingerprintDeviceIpAddressDuplicate = await repositoryManager
                .FingerprintDeviceRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.IpAddress == model.IpAddress && c.Id != model.Id).AnyAsync();
            if (checkFingerprintDeviceIpAddressDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceIpAddressIsDuplicated);
            }

            return true;
        }
    }
}
