using Dawem.Contract.BusinessValidation.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Permissions.Permissions;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Permissions
{

    public class PermissionBLValidation : IPermissionBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public PermissionBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreatePermissionModel model)
        {
            var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.RoleId == model.RoleId).AnyAsync();
            if (checkPermissionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionRoleIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdatePermissionModel model)
        {
            var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.RoleId == model.RoleId && c.Id != model.Id).AnyAsync();
            if (checkPermissionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionRoleIsDuplicated);
            }

            return true;
        }
    }
}
