using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultPermissionsTypeBLValidation : IDefaultPermissionTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultPermissionsTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultPermissionsTypeDTO model)
        {
            var checkPermissionsTypeDuplicate = await repositoryManager
                .DefaultPermissionTypeRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.PermissionsTypes).AnyAsync();
            if (checkPermissionsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisPermission);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultPermissionsTypeDTO model)
        {
            var checkPermissionsTypeDuplicate = await repositoryManager
                .DefaultPermissionTypeRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.PermissionsTypes).AnyAsync();
            if (checkPermissionsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisPermission);
            }

            return true;
        }


    }
}
