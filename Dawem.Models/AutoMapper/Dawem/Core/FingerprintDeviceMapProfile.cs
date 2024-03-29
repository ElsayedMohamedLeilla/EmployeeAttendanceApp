using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Attendances.FingerprintDevices;

namespace Dawem.Models.AutoMapper.Dawem.Core
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
