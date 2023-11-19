using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.Roles;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Core.Roles
{
    public class RoleBL : IRoleBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public RoleBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext
        )
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            mapper = _mapper;
        }

        public async Task<GetRoleDropDownResponseDTO> GetForDropDown(GetRolesCriteria criteria)
        {
            criteria.IsActive = true;
            var RoleRepository = repositoryManager.RoleRepository;
            var query = RoleRepository.GetAsQueryable(criteria);
            #region Handle Response

            var RolesList = await query.Select(e => new GetRoleForDropDownResponseModelDTO
            {
                Id = e.Name,
                Name = TranslationHelper.GetTranslation(e.Name, requestInfo.Lang)
            }).ToListAsync();

            return new GetRoleDropDownResponseDTO
            {
                Roles = RolesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


    }
}
