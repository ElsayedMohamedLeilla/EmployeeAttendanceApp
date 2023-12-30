using Dawem.Contract.BusinessLogic.Others;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Others
{
    public class PermissionBL : IPermissionBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;

        public PermissionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager, RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            repositoryManager = _repositoryManager;
            requestInfo = _requestHeaderContext;
        }
        public async Task<bool> CheckUserPermission(CheckUserPermissionModel model)
        {
            var permissionRepository = repositoryManager.PermissionRepository;
            var permissionScreenActionRepository = repositoryManager.PermissionScreenActionRepository;
            var userRepository = repositoryManager.UserRepository;

            var getUserRoles = await userRepository.Get(u => !u.IsDeleted && u.Id == model.UserId)
                .Select(u => new { UserRoles = u.UserRoles.Select(ur => ur.RoleId) })
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            var roles = getUserRoles.UserRoles.Select(roleId => roleId).ToList();

            if (roles != null && roles.Count > 0)
            {
                var checkIfHasPermission = await permissionScreenActionRepository
                    .Get(p => !p.IsDeleted && !p.PermissionScreen.IsDeleted && !p.PermissionScreen.Permission.IsDeleted &&
                    p.PermissionScreen.Permission.CompanyId == requestInfo.CompanyId &&
                    p.PermissionScreen.ScreenCode == model.Screen
                     && p.Action == model.Action && roles.Contains(p.PermissionScreen.Permission.RoleId))
                    .AnyAsync();

                if (!checkIfHasPermission)
                {
                    var checkIfHasAnyPermissino = await permissionRepository
                        .Get(p => !p.IsDeleted && roles.Contains(p.RoleId)).AnyAsync();
                    if (checkIfHasAnyPermissino)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

}

