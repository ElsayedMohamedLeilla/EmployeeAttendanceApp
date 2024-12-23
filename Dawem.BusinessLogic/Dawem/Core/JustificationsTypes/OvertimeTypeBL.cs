using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.VacationsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.OvertimesTypes
{
    public class OvertimeTypeBL : IOvertimeTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IOvertimesTypeBLValidation overtimeTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public OvertimeTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IOvertimesTypeBLValidation _overtimesTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            overtimeTypeBLValidation = _overtimesTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateOvertimeTypeDTO model)
        {
            #region Business Validation

            await overtimeTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert OvertimesType

            #region Set OvertimesType code

            var getNextCode = await repositoryManager.OvertimeTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var overtimesType = mapper.Map<OvertimeType>(model);
            overtimesType.CompanyId = requestInfo.CompanyId;
            overtimesType.AddUserId = requestInfo.UserId;
            overtimesType.Code = getNextCode;
            repositoryManager.OvertimeTypeRepository.Insert(overtimesType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return overtimesType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateOvertimeTypeDTO model)
        {
            #region Business Validation

            await overtimeTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();
            #region Update OvertimesType
            var getOvertimesType = await repositoryManager.OvertimeTypeRepository.GetByIdAsync(model.Id);
            getOvertimesType.Name = model.Name;
            getOvertimesType.IsActive = model.IsActive;
            getOvertimesType.ModifiedDate = DateTime.Now;
            getOvertimesType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetOvertimesTypeResponseDTO> Get(GetOvertimeTypesCriteria criteria)
        {
            var overtimesTypeRepository = repositoryManager.OvertimeTypeRepository;
            var query = overtimesTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = overtimesTypeRepository.OrderBy(query, nameof(OvertimeType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var overtimesTypesList = await queryPaged.Select(e => new GetOvertimesTypeResponseModelDTO
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();

            return new GetOvertimesTypeResponseDTO
            {
                OvertimesTypes = overtimesTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetOvertimesTypeDropDownResponseDTO> GetForDropDown(GetOvertimeTypesCriteria criteria)
        {
            criteria.IsActive = true;
            var OvertimeTypeRepository = repositoryManager.OvertimeTypeRepository;
            var query = OvertimeTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = OvertimeTypeRepository.OrderBy(query, nameof(OvertimeType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var overtimesTypesList = await queryPaged.Select(e => new GetOvertimesTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetOvertimesTypeDropDownResponseDTO
            {
                OvertimesTypes = overtimesTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetOvertimesTypeInfoResponseDTO> GetInfo(int overtimesTypeId)
        {
            var overtimesType = await repositoryManager.OvertimeTypeRepository.Get(e => e.Id == overtimesTypeId && !e.IsDeleted)
                .Select(e => new GetOvertimesTypeInfoResponseDTO
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryOvertimesTypeNotFound);

            return overtimesType;
        }
        public async Task<GetOvertimesTypeByIdResponseDTO> GetById(int overtimesTypeId)
        {
            var overtimesType = await repositoryManager.OvertimeTypeRepository.Get(e => e.Id == overtimesTypeId && !e.IsDeleted)
                .Select(e => new GetOvertimesTypeByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryOvertimesTypeNotFound);

            return overtimesType;

        }
        public async Task<bool> Delete(int OvertimesTypeId)
        {
            var overtimesType = await repositoryManager.OvertimeTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == OvertimesTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryOvertimesTypeNotFound);
            overtimesType.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetOvertimesTypesInformationsResponseDTO> GetOvertimeTypesInformations()
        {
            var overtimeRepository = repositoryManager.OvertimeTypeRepository;
            var query = overtimeRepository.Get(overtime => overtime.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetOvertimesTypesInformationsResponseDTO
            {
                TotalCount = await query.Where(overtime => !overtime.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(overtime => !overtime.IsDeleted && overtime.IsActive).CountAsync(),
                NotActiveCount = await query.Where(overtime => !overtime.IsDeleted && !overtime.IsActive).CountAsync(),
                DeletedCount = await query.Where(overtime => overtime.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}
