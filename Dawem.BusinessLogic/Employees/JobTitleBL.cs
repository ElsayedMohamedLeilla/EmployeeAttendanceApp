using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.JobTitle;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.JobTitles;
using Dawem.Models.Response.Employees.TaskTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class JobTitleBL : IJobTitleBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IJobTitleBLValidation departmentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public JobTitleBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IJobTitleBLValidation _departmentBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            departmentBLValidation = _departmentBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateJobTitleModel model)
        {
            #region Business Validation

            await departmentBLValidation.CreateValidation(model);

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

            var department = mapper.Map<JobTitle>(model);
            department.CompanyId = requestInfo.CompanyId;
            department.AddUserId = requestInfo.UserId;

            department.Code = getNextCode;
            repositoryManager.JobTitleRepository.Insert(department);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return department.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateJobTitleModel model)
        {
            #region Business Validation
            await departmentBLValidation.UpdateValidation(model);
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
            var departmentRepository = repositoryManager.JobTitleRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(JobTitle.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetJobTitlesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetJobTitlesResponse
            {
                JobTitles = departmentsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetJobTitlesForDropDownResponse> GetForDropDown(GetJobTitlesCriteria criteria)
        {
            criteria.IsActive = true;
            var departmentRepository = repositoryManager.JobTitleRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(JobTitle.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetJobTitlesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetJobTitlesForDropDownResponse
            {
                JobTitles = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetJobTitleInfoResponseModel> GetInfo(int JobTitleId)
        {
            var department = await repositoryManager.JobTitleRepository.Get(e => e.Id == JobTitleId && !e.IsDeleted)
                .Select(e => new GetJobTitleInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);

            return department;
        }
        public async Task<GetJobTitleByIdResponseModel> GetById(int JobTitleId)
        {
            var department = await repositoryManager.JobTitleRepository.Get(e => e.Id == JobTitleId && !e.IsDeleted)
                .Select(e => new GetJobTitleByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);

            return department;

        }
        public async Task<bool> Delete(int departmentd)
        {
            var department = await repositoryManager.JobTitleRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == departmentd) ??
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);
            department.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

