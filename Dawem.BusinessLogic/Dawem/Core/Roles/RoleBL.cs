using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Response.Core.Roles;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.Roles
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
            var roleRepository = repositoryManager.RoleRepository;
            var query = roleRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = roleRepository.OrderBy(query, nameof(Role.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var RolesList = await queryPaged.Select(e => new GetRoleForDropDownResponseModelDTO
            {
                Id = e.Id,
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
