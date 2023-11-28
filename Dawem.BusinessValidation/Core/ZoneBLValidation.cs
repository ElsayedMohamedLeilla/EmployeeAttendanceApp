using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Core.Zones;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.Core
{
    public class ZoneBLValidation : IZoneBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public ZoneBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateZoneDTO model)
        {
            var checkZoneDuplicate = await repositoryManager
                .ZoneRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();

            if (checkZoneDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryZoneNameIsDuplicated);
            }
            var checkLatDuplicate = await repositoryManager
               .ZoneRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Latitude == model.Latitude).AnyAsync();
            if (checkLatDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryLatitudeIsUsedInAnotherZone);
            }
            var checkLongDuplicate = await repositoryManager
             .ZoneRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Longitude == model.Latitude).AnyAsync();
            if (checkLongDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryLongitudeIsUsedInAnotherZone);
            }
            return true;
        }


        public async Task<bool> UpdateValidation(UpdateZoneDTO model)
        {
            var checkZoneDuplicate = await repositoryManager
                .ZoneRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkZoneDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryZoneNameIsDuplicated);
            }


            return true;
        }


    }
}
