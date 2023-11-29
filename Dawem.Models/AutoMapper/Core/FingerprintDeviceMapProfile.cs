using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Attendances.FingerprintDevices;

namespace Dawem.Models.AutoMapper.Core
{
    public class FingerprintDeviceMapProfile : Profile
    {
        public FingerprintDeviceMapProfile()
        {
            CreateMap<CreateFingerprintDeviceModel, FingerprintDevice>();
            CreateMap<UpdateFingerprintDeviceModel, FingerprintDevice>();
        }
    }
}
