using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.AssignmentType;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.AssignmentTypes;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Employees;
using Dawem.Validation.FluentValidation.Employees.AssignmentTypes;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class AssignmentTypeBL : IAssignmentTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IAssignmentTypeBLValidation departmentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public AssignmentTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IAssignmentTypeBLValidation _departmentBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            departmentBLValidation = _departmentBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateAssignmentTypeModel model)
        {
            #region Business Validation

            await departmentBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert AssignmentType

            #region Set AssignmentType code
            var getNextCode = await repositoryManager.AssignmentTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var department = mapper.Map<AssignmentType>(model);
            department.CompanyId = requestInfo.CompanyId;
            department.AddUserId = requestInfo.UserId;

            department.Code = getNextCode;
            repositoryManager.AssignmentTypeRepository.Insert(department);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return department.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateAssignmentTypeModel model)
        {
            #region Business Validation

            await departmentBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update AssignmentType

            var getAssignmentType = await repositoryManager.AssignmentTypeRepository
                .GetEntityByConditionWithTrackingAsync(assignmentType => !assignmentType.IsDeleted
                && assignmentType.Id == model.Id);


            if (getAssignmentType != null)
            {
                getAssignmentType.Name = model.Name;
                getAssignmentType.IsActive = model.IsActive;
                getAssignmentType.ModifiedDate = DateTime.Now;
                getAssignmentType.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(DawemKeys.SorryAssignmentTypeNotFound);


        }
        public async Task<GetAssignmentTypesResponse> Get(GetAssignmentTypesCriteria criteria)
        {
            var departmentRepository = repositoryManager.AssignmentTypeRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(AssignmentType.Id), DawemKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetAssignmentTypesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetAssignmentTypesResponse
            {
                AssignmentTypes = departmentsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetAssignmentTypesForDropDownResponse> GetForDropDown(GetAssignmentTypesCriteria criteria)
        {

            criteria.IsActive = true;
            var departmentRepository = repositoryManager.AssignmentTypeRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(AssignmentType.Id), DawemKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetAssignmentTypesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetAssignmentTypesForDropDownResponse
            {
                AssignmentTypes = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetAssignmentTypeInfoResponseModel> GetInfo(int AssignmentTypeId)
        {
            var department = await repositoryManager.AssignmentTypeRepository.Get(e => e.Id == AssignmentTypeId && !e.IsDeleted)
                .Select(e => new GetAssignmentTypeInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryAssignmentTypeNotFound);

            return department;
        }
        public async Task<GetAssignmentTypeByIdResponseModel> GetById(int AssignmentTypeId)
        {
            var department = await repositoryManager.AssignmentTypeRepository.Get(e => e.Id == AssignmentTypeId && !e.IsDeleted)
                .Select(e => new GetAssignmentTypeByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryAssignmentTypeNotFound);

            return department;

        }
        public async Task<bool> Delete(int departmentd)
        {
            var department = await repositoryManager.AssignmentTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == departmentd) ??
                throw new BusinessValidationException(DawemKeys.SorryAssignmentTypeNotFound);
            department.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

