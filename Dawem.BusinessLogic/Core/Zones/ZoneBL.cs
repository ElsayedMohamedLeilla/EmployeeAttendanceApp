using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Zones;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.Zones;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Core.Zones
{
    public class ZoneBL : IZoneBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IZoneBLValidation ZoneBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;


        public ZoneBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
         IUploadBLC _uploadBLC,
        IZoneBLValidation _ZoneBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            ZoneBLValidation = _ZoneBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;

        }
        public async Task<int> Create(CreateZoneDTO model)
        {
            #region Business Validation
            await ZoneBLValidation.CreateValidation(model);
            #endregion
            unitOfWork.CreateTransaction();
            #region Insert Zone
            #region Set Zone code
            var getNextCode = await repositoryManager.ZoneRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion
            var Zone = mapper.Map<Zone>(model);
            Zone.CompanyId = requestInfo.CompanyId;
            Zone.AddUserId = requestInfo.UserId;
            Zone.AddedApplicationType = requestInfo.ApplicationType;
            Zone.Code = getNextCode;
            repositoryManager.ZoneRepository.Insert(Zone);
            await unitOfWork.SaveAsync();
            #endregion
            #region Handle Response
            await unitOfWork.CommitAsync();
            return Zone.Id;
            #endregion

        }
        public async Task<bool> Update(UpdateZoneDTO model)
        {
            #region Business Validation

            await ZoneBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Zone

            var getZone = await repositoryManager.ZoneRepository.GetEntityByConditionWithTrackingAsync(grp => !grp.IsDeleted
            && grp.Id == model.Id);

            if (getZone != null)
            {
                getZone.Name = model.Name;
                getZone.IsActive = model.IsActive;
                getZone.Latitude = model.Latitude;
                getZone.Longitude = model.Longitude;
                getZone.Radius = model.Radius;
                getZone.ModifiedDate = DateTime.UtcNow;
                getZone.ModifyUserId = requestInfo.UserId;
                getZone.ModifiedApplicationType = requestInfo.ApplicationType;
            }
            else
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            #endregion

            await unitOfWork.SaveAsync();

            #region Handle Response

            await unitOfWork.CommitAsync();

            return true;

            #endregion

        }
        public async Task<GetZoneResponseDTO> Get(GetZoneCriteria criteria)
        {
            var ZoneRepository = repositoryManager.ZoneRepository;
            var query = ZoneRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ZoneRepository.OrderBy(query, nameof(Zone.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response
            var ZonesList = await queryPaged.Select(Zone => new GetZoneResponseModelDTO
            {
                Id = Zone.Id,
                Code = Zone.Code,
                Name = Zone.Name,
                IsActive = Zone.IsActive,
                Latitude = Zone.Latitude,
                Longitude = Zone.Longitude,
                Radius = Zone.Radius
            }).ToListAsync();



            return new GetZoneResponseDTO
            {
                Zones = ZonesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetZoneDropDownResponseDTO> GetForDropDown(GetZoneCriteria criteria)
        {
            criteria.IsActive = true;
            var ZoneRepository = repositoryManager.ZoneRepository;
            var query = ZoneRepository.GetAsQueryable(criteria);
            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ZoneRepository.OrderBy(query, nameof(Zone.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion
            #region Handle Response

            var ZonesList = await queryPaged.Select(e => new GetZoneForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetZoneDropDownResponseDTO
            {
                Zones = ZonesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetZoneInfoResponseDTO> GetInfo(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository.Get(e => e.Id == ZoneId && !e.IsDeleted)
           .Select(Zone => new GetZoneInfoResponseDTO
           {
               Code = Zone.Code,
               Name = Zone.Name,
               IsActive = Zone.IsActive,
               Radius = Zone.Radius,
               Latitude = Zone.Latitude,
               Longitude = Zone.Longitude,
           }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            return Zone;
        }
        public async Task<GetZoneByIdResponseDTO> GetById(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository.Get(e => e.Id == ZoneId && !e.IsDeleted)
                .Select(Zone => new GetZoneByIdResponseDTO
                {
                    Id = Zone.Id,
                    Name = Zone.Name,
                    IsActive = Zone.IsActive,
                    Longitude = Zone.Longitude,
                    Latitude = Zone.Latitude,
                    Radius = Zone.Radius
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            return Zone;

        }
        public async Task<bool> Delete(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == ZoneId) ??
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            Zone.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == ZoneId) ??
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);
            Zone.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var Zone = await repositoryManager.ZoneRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);
            Zone.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }

    }
}
