using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.VacationsTypes;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Core.VacationsTypes;
using Dawem.Validation.FluentValidation.Employees;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Core.VacationsTypes
{
    public class VacationTypeBL : IVacationTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IVacationTypeBLValidation vacationTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public VacationTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IVacationTypeBLValidation _VacationsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            vacationTypeBLValidation = _VacationsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateVacationsTypeDTO model)
        {
            #region Business Validation

            await vacationTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert VacationsType

            #region Set VacationsType code

            var getNextCode = await repositoryManager.VacationsTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var VacationsType = mapper.Map<VacationType>(model);
            VacationsType.CompanyId = requestInfo.CompanyId;
            VacationsType.AddUserId = requestInfo.UserId;
            VacationsType.Code = getNextCode;
            repositoryManager.VacationsTypeRepository.Insert(VacationsType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return VacationsType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateVacationsTypeDTO model)
        {
            #region Business Validation

            await vacationTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();
            #region Update VacationsType
            var getVacationsType = await repositoryManager.VacationsTypeRepository.GetByIdAsync(model.Id);
            getVacationsType.Name = model.Name;
            getVacationsType.IsActive = model.IsActive;
            getVacationsType.ModifiedDate = DateTime.Now;
            getVacationsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetVacationsTypeResponseDTO> Get(GetVacationsTypesCriteria criteria)
        {
            var VacationsTypeRepository = repositoryManager.VacationsTypeRepository;
            var query = VacationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = VacationsTypeRepository.OrderBy(query, nameof(VacationType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var VacationsTypesList = await queryPaged.Select(e => new GetVacationsTypeResponseModelDTO
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();

            return new GetVacationsTypeResponseDTO
            {
                VacationsTypes = VacationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetVacationsTypeDropDownResponseDTO> GetForDropDown(GetVacationsTypesCriteria criteria)
        {
            criteria.IsActive = true;
            var VacationsTypeRepository = repositoryManager.VacationsTypeRepository;
            var query = VacationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = VacationsTypeRepository.OrderBy(query, nameof(VacationType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var VacationsTypesList = await queryPaged.Select(e => new GetVacationsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetVacationsTypeDropDownResponseDTO
            {
                VacationsTypes = VacationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetVacationsTypeInfoResponseDTO> GetInfo(int VacationsTypeId)
        {
            var VacationsType = await repositoryManager.VacationsTypeRepository.Get(e => e.Id == VacationsTypeId && !e.IsDeleted)
                .Select(e => new GetVacationsTypeInfoResponseDTO
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            return VacationsType;
        }
        public async Task<GetVacationsTypeByIdResponseDTO> GetById(int VacationsTypeId)
        {
            var VacationsType = await repositoryManager.VacationsTypeRepository.Get(e => e.Id == VacationsTypeId && !e.IsDeleted)
                .Select(e => new GetVacationsTypeByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            return VacationsType;

        }
        public async Task<bool> Delete(int VacationsTypeId)
        {
            var vacationsType = await repositoryManager.VacationsTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == VacationsTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);
            vacationsType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

    }
}
