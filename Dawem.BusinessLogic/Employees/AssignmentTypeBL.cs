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
using Dawem.Models.Response.Requests.Vacations;
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
        private readonly IAssignmentTypeBLValidation assignmentTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public AssignmentTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IAssignmentTypeBLValidation _assignmentTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            assignmentTypeBLValidation = _assignmentTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateAssignmentTypeModel model)
        {
            #region Business Validation

            await assignmentTypeBLValidation.CreateValidation(model);

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

            var assignmentType = mapper.Map<AssignmentType>(model);
            assignmentType.CompanyId = requestInfo.CompanyId;
            assignmentType.AddUserId = requestInfo.UserId;

            assignmentType.Code = getNextCode;
            repositoryManager.AssignmentTypeRepository.Insert(assignmentType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return assignmentType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateAssignmentTypeModel model)
        {
            #region Business Validation

            await assignmentTypeBLValidation.UpdateValidation(model);

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
                throw new BusinessValidationException(LeillaKeys.SorryAssignmentTypeNotFound);


        }
        public async Task<GetAssignmentTypesResponse> Get(GetAssignmentTypesCriteria criteria)
        {
            var assignmentTypeRepository = repositoryManager.AssignmentTypeRepository;
            var query = assignmentTypeRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = assignmentTypeRepository.OrderBy(query, nameof(AssignmentType.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var assignmentTypesList = await queryPaged.Select(e => new GetAssignmentTypesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetAssignmentTypesResponse
            {
                AssignmentTypes = assignmentTypesList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetAssignmentTypesForDropDownResponse> GetForDropDown(GetAssignmentTypesCriteria criteria)
        {

            criteria.IsActive = true;
            var assignmentTypeRepository = repositoryManager.AssignmentTypeRepository;
            var query = assignmentTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = assignmentTypeRepository.OrderBy(query, nameof(AssignmentType.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var assignmentTypesList = await queryPaged.Select(e => new GetAssignmentTypesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetAssignmentTypesForDropDownResponse
            {
                AssignmentTypes = assignmentTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetAssignmentTypeInfoResponseModel> GetInfo(int AssignmentTypeId)
        {
            var assignmentType = await repositoryManager.AssignmentTypeRepository.Get(e => e.Id == AssignmentTypeId && !e.IsDeleted)
                .Select(e => new GetAssignmentTypeInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryAssignmentTypeNotFound);

            return assignmentType;
        }
        public async Task<GetAssignmentTypeByIdResponseModel> GetById(int AssignmentTypeId)
        {
            var assignmentType = await repositoryManager.AssignmentTypeRepository.Get(e => e.Id == AssignmentTypeId && !e.IsDeleted)
                .Select(e => new GetAssignmentTypeByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryAssignmentTypeNotFound);

            return assignmentType;

        }
        public async Task<bool> Delete(int assignmentTyped)
        {
            var assignmentType = await repositoryManager.AssignmentTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == assignmentTyped) ??
                throw new BusinessValidationException(LeillaKeys.SorryAssignmentTypeNotFound);
            assignmentType.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetAssignmentTypesInformationsResponseDTO> GetAssignmentTypesInformations()
        {
            var assignmentTypeRepository = repositoryManager.AssignmentTypeRepository;
            var query = assignmentTypeRepository.Get(assignmentType => assignmentType.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetAssignmentTypesInformationsResponseDTO
            {
                TotalCount = await query.Where(assignmentType => !assignmentType.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(assignmentType => !assignmentType.IsDeleted && assignmentType.IsActive).CountAsync(),
                NotActiveCount = await query.Where(assignmentType => !assignmentType.IsDeleted && !assignmentType.IsActive).CountAsync(),
                DeletedCount = await query.Where(assignmentType => assignmentType.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

