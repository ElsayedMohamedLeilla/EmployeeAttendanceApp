using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.HolidayTypes;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.HolidayTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class HolidayTypeBL : IHolidayTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IHolidayTypeBLValidation departmentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public HolidayTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IHolidayTypeBLValidation _departmentBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            departmentBLValidation = _departmentBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateHolidayTypeModel model)
        {
            #region Business Validation

            await departmentBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert HolidayType

            #region Set HolidayType code
            var getNextCode = await repositoryManager.HolidayTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var department = mapper.Map<HolidayType>(model);
            department.CompanyId = requestInfo.CompanyId;
            department.AddUserId = requestInfo.UserId;

            department.Code = getNextCode;
            repositoryManager.HolidayTypeRepository.Insert(department);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return department.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateHolidayTypeModel model)
        {
            #region Business Validation
            await departmentBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Update HolidayType

            var getHolidayType = await repositoryManager.HolidayTypeRepository
                .GetEntityByConditionWithTrackingAsync(holidayType => !holidayType.IsDeleted
                && holidayType.Id == model.Id);

            if (getHolidayType != null)
            {
                getHolidayType.Name = model.Name;
                getHolidayType.IsActive = model.IsActive;
                getHolidayType.ModifiedDate = DateTime.Now;
                getHolidayType.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryHolidayTypeNotFound);


        }
        public async Task<GetHolidayTypesResponse> Get(GetHolidayTypesCriteria criteria)
        {
            var departmentRepository = repositoryManager.HolidayTypeRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(HolidayType.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetHolidayTypesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetHolidayTypesResponse
            {
                HolidayTypes = departmentsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetHolidayTypesForDropDownResponse> GetForDropDown(GetHolidayTypesCriteria criteria)
        {
            criteria.IsActive = true;
            var departmentRepository = repositoryManager.HolidayTypeRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(HolidayType.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetHolidayTypesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetHolidayTypesForDropDownResponse
            {
                HolidayTypes = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetHolidayTypeInfoResponseModel> GetInfo(int HolidayTypeId)
        {
            var department = await repositoryManager.HolidayTypeRepository.Get(e => e.Id == HolidayTypeId && !e.IsDeleted)
                .Select(e => new GetHolidayTypeInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryHolidayTypeNotFound);

            return department;
        }
        public async Task<GetHolidayTypeByIdResponseModel> GetById(int HolidayTypeId)
        {
            var department = await repositoryManager.HolidayTypeRepository.Get(e => e.Id == HolidayTypeId && !e.IsDeleted)
                .Select(e => new GetHolidayTypeByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryHolidayTypeNotFound);

            return department;

        }
        public async Task<bool> Delete(int departmentd)
        {
            var department = await repositoryManager.HolidayTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == departmentd) ??
                throw new BusinessValidationException(LeillaKeys.SorryHolidayTypeNotFound);
            department.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetHolidayTypesInformationsResponseDTO> GetHolidayTypesInformations()
        {
            var holidayTypeRepository = repositoryManager.HolidayTypeRepository;
            var query = holidayTypeRepository.Get(holidayType => holidayType.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetHolidayTypesInformationsResponseDTO
            {
                TotalCount = await query.Where(holidayType => !holidayType.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(holidayType => !holidayType.IsDeleted && holidayType.IsActive).CountAsync(),
                NotActiveCount = await query.Where(holidayType => !holidayType.IsDeleted && !holidayType.IsActive).CountAsync(),
                DeletedCount = await query.Where(holidayType => holidayType.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

