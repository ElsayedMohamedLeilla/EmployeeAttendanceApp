using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Core
{
    public class PermissionsTypeBLValidation : IPermissionsTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public PermissionsTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreatePermissionTypeDTO model)
        {
            var checkPermissionsTypeDuplicate = await repositoryManager
                .PermissionsTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkPermissionsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNameIsDuplicated);
            }

            return true;
        }


        public async Task<bool> UpdateValidation(UpdatePermissionTypeDTO model)
        {
            var checkPermissionsTypeDuplicate = await repositoryManager
                .PermissionsTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkPermissionsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNameIsDuplicated);
            }

            return true;
        }


    }
}
