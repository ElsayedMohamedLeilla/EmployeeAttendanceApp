using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.JobTitles;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Employees.JobTitles;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Employees
{
    public class JobTitleBL : IJobTitleBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IJobTitleBLValidation responsibilityBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public JobTitleBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IJobTitleBLValidation _responsibilityBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            responsibilityBLValidation = _responsibilityBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateJobTitleModel model)
        {
            #region Business Validation

            await responsibilityBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert JobTitle

            #region Set JobTitle code
            var getNextCode = await repositoryManager.JobTitleRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var responsibility = mapper.Map<JobTitle>(model);
            responsibility.CompanyId = requestInfo.CompanyId;
            responsibility.AddUserId = requestInfo.UserId;

            responsibility.Code = getNextCode;
            repositoryManager.JobTitleRepository.Insert(responsibility);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return responsibility.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateJobTitleModel model)
        {
            #region Business Validation
            await responsibilityBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Update JobTitle

            var getJobTitle = await repositoryManager.JobTitleRepository
                 .GetEntityByConditionWithTrackingAsync(jobTitle => !jobTitle.IsDeleted
                 && jobTitle.Id == model.Id);

            if (getJobTitle != null)
            {
                getJobTitle.Name = model.Name;
                getJobTitle.IsActive = model.IsActive;
                getJobTitle.ModifiedDate = DateTime.Now;
                getJobTitle.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);


        }
        public async Task<GetJobTitlesResponse> Get(GetJobTitlesCriteria criteria)
        {
            var responsibilityRepository = repositoryManager.JobTitleRepository;
            var query = responsibilityRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = responsibilityRepository.OrderBy(query, nameof(JobTitle.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var responsibilitysList = await queryPaged.Select(e => new GetJobTitlesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetJobTitlesResponse
            {
                JobTitles = responsibilitysList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetJobTitlesForDropDownResponse> GetForDropDown(GetJobTitlesCriteria criteria)
        {
            criteria.IsActive = true;
            var responsibilityRepository = repositoryManager.JobTitleRepository;
            var query = responsibilityRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = responsibilityRepository.OrderBy(query, nameof(JobTitle.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var responsibilitysList = await queryPaged.Select(e => new GetJobTitlesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetJobTitlesForDropDownResponse
            {
                JobTitles = responsibilitysList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetJobTitleInfoResponseModel> GetInfo(int JobTitleId)
        {
            var responsibility = await repositoryManager.JobTitleRepository.Get(e => e.Id == JobTitleId && !e.IsDeleted)
                .Select(e => new GetJobTitleInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);

            return responsibility;
        }
        public async Task<GetJobTitleByIdResponseModel> GetById(int JobTitleId)
        {
            var responsibility = await repositoryManager.JobTitleRepository.Get(e => e.Id == JobTitleId && !e.IsDeleted)
                .Select(e => new GetJobTitleByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);

            return responsibility;

        }
        public async Task<bool> Delete(int responsibilityd)
        {
            var responsibility = await repositoryManager.JobTitleRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == responsibilityd) ??
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);
            responsibility.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetJobTitlesInformationsResponseDTO> GetJobTitlesInformations()
        {
            var jobTitleRepository = repositoryManager.JobTitleRepository;
            var query = jobTitleRepository.Get(jobTitle => jobTitle.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetJobTitlesInformationsResponseDTO
            {
                TotalCount = await query.CountAsync(),
                ActiveCount = await query.Where(jobTitle => !jobTitle.IsDeleted && jobTitle.IsActive).CountAsync(),
                NotActiveCount = await query.Where(jobTitle => !jobTitle.IsDeleted && !jobTitle.IsActive).CountAsync(),
                DeletedCount = await query.Where(jobTitle => jobTitle.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

