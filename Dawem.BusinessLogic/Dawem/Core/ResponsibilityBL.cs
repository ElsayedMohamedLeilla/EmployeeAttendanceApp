using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Employees.JobTitles;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Employees
{
    public class ResponsibilityBL : IResponsibilityBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IResponsibilityBLValidation responsibilityBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public ResponsibilityBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IResponsibilityBLValidation _responsibilityBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            responsibilityBLValidation = _responsibilityBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateResponsibilityModel model)
        {
            #region Business Validation

            await responsibilityBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Responsibility

            #region Set Responsibility code
            var getNextCode = await repositoryManager.ResponsibilityRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var responsibility = mapper.Map<Responsibility>(model);
            responsibility.CompanyId = requestInfo.CompanyId > 0 ? requestInfo.CompanyId : null;
            responsibility.AddUserId = requestInfo.UserId;

            responsibility.Code = getNextCode;
            repositoryManager.ResponsibilityRepository.Insert(responsibility);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return responsibility.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateResponsibilityModel model)
        {
            #region Business Validation
            await responsibilityBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Update Responsibility

            var getResponsibility = await repositoryManager.ResponsibilityRepository
                 .GetEntityByConditionWithTrackingAsync(responsibility => !responsibility.IsDeleted
                 && responsibility.Id == model.Id);

            if (getResponsibility != null)
            {
                getResponsibility.Name = model.Name;
                getResponsibility.IsActive = model.IsActive;
                getResponsibility.ModifiedDate = DateTime.Now;
                getResponsibility.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNotFound);


        }
        public async Task<GetResponsibilitiesResponse> Get(GetResponsibilitiesCriteria criteria)
        {
            var responsibilityRepository = repositoryManager.ResponsibilityRepository;
            var query = responsibilityRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = responsibilityRepository.OrderBy(query, nameof(Responsibility.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var responsibilitysList = await queryPaged.Select(e => new GetResponsibilitiesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetResponsibilitiesResponse
            {
                Responsibilities = responsibilitysList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetResponsibilitiesForDropDownResponse> GetForDropDown(GetResponsibilitiesCriteria criteria)
        {
            criteria.IsActive = true;
            var responsibilityRepository = repositoryManager.ResponsibilityRepository;
            var query = responsibilityRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = responsibilityRepository.OrderBy(query, nameof(Responsibility.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var responsibilitysList = await queryPaged.Select(e => new GetResponsibilitiesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetResponsibilitiesForDropDownResponse
            {
                Responsibilities = responsibilitysList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetResponsibilityInfoResponseModel> GetInfo(int ResponsibilityId)
        {
            var responsibility = await repositoryManager.ResponsibilityRepository.Get(e => e.Id == ResponsibilityId && !e.IsDeleted)
                .Select(e => new GetResponsibilityInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNotFound);

            return responsibility;
        }
        public async Task<GetResponsibilityByIdResponseModel> GetById(int ResponsibilityId)
        {
            var responsibility = await repositoryManager.ResponsibilityRepository.Get(e => e.Id == ResponsibilityId && !e.IsDeleted)
                .Select(e => new GetResponsibilityByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNotFound);

            return responsibility;

        }
        public async Task<bool> Delete(int responsibilityd)
        {
            var responsibility = await repositoryManager.ResponsibilityRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == responsibilityd) ??
                throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNotFound);
            responsibility.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetResponsibilitiesInformationsResponseDTO> GetResponsibilitiesInformations()
        {
            var responsibilityRepository = repositoryManager.ResponsibilityRepository;
            var query = responsibilityRepository.
                Get(responsibility => responsibility.CompanyId == requestInfo.CompanyId || 
                responsibility.CompanyId == null);

            #region Handle Response

            return new GetResponsibilitiesInformationsResponseDTO
            {
                TotalCount = await query.CountAsync(),
                ActiveCount = await query.Where(responsibility => !responsibility.IsDeleted && responsibility.IsActive).CountAsync(),
                NotActiveCount = await query.Where(responsibility => !responsibility.IsDeleted && !responsibility.IsActive).CountAsync(),
                DeletedCount = await query.Where(responsibility => responsibility.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

