using AutoMapper;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogicCore.Dawem
{
    public class PermissionBLC : IPermissionBLC
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IPermissionBLValidation permissionBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public PermissionBLC(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IPermissionBLValidation _permissionBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            permissionBLValidation = _permissionBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreatePermissionModel model)
        {
            #region Business Validation

            await permissionBLValidation.CreateValidation(model);

            #endregion

            #region Insert Permission

            #region Set Permission Code
            var getNextCode = await repositoryManager.PermissionRepository
                .Get(permission => permission.AuthenticationType == requestInfo.AuthenticationType &&
                (requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId ||
                requestInfo.CompanyId <= 0 && permission.CompanyId == null))
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var permission = mapper.Map<Permission>(model);
            permission.CompanyId = requestInfo.CompanyId > 0 ? requestInfo.CompanyId : null;
            permission.AddUserId = requestInfo.UserId;
            permission.AuthenticationType = requestInfo.AuthenticationType;
            permission.Code = getNextCode;
            repositoryManager.PermissionRepository.Insert(permission);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            return permission.Id;

            #endregion

        }
    }
}

