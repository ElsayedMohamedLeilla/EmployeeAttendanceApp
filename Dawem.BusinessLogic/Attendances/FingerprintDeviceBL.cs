using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances.FingerprintDevices;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.TaskTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class FingerprintDeviceBL : IFingerprintDeviceBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IFingerprintDeviceBLValidation fingerprintDeviceBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public FingerprintDeviceBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IFingerprintDeviceBLValidation _fingerprintDeviceBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            fingerprintDeviceBLValidation = _fingerprintDeviceBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateFingerprintDeviceModel model)
        {
            #region Business Validation

            await fingerprintDeviceBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert FingerprintDevice

            #region Set FingerprintDevice code
            var getNextCode = await repositoryManager.FingerprintDeviceRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var fingerprintDevice = mapper.Map<FingerprintDevice>(model);
            fingerprintDevice.CompanyId = requestInfo.CompanyId;
            fingerprintDevice.AddUserId = requestInfo.UserId;

            fingerprintDevice.Code = getNextCode;
            repositoryManager.FingerprintDeviceRepository.Insert(fingerprintDevice);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return fingerprintDevice.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateFingerprintDeviceModel model)
        {
            #region Business Validation
            await fingerprintDeviceBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Update FingerprintDevice

            var getFingerprintDevice = await repositoryManager.FingerprintDeviceRepository
                 .GetEntityByConditionWithTrackingAsync(fingerprintDevice => !fingerprintDevice.IsDeleted
                 && fingerprintDevice.Id == model.Id);

            if (getFingerprintDevice != null)
            {
                getFingerprintDevice.Name = model.Name;
                getFingerprintDevice.IpAddress = model.IpAddress;
                getFingerprintDevice.SerialNumber = model.SerialNumber;
                getFingerprintDevice.Model = model.Model;
                getFingerprintDevice.PortNumber = model.PortNumber;
                getFingerprintDevice.IsActive = model.IsActive;
                getFingerprintDevice.ModifiedDate = DateTime.Now;
                getFingerprintDevice.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceNotFound);

        }
        public async Task<GetFingerprintDevicesResponse> Get(GetFingerprintDevicesCriteria criteria)
        {
            var fingerprintDeviceRepository = repositoryManager.FingerprintDeviceRepository;
            var query = fingerprintDeviceRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = fingerprintDeviceRepository.OrderBy(query, nameof(FingerprintDevice.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var fingerprintDevicesList = await queryPaged.Select(e => new GetFingerprintDevicesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IpAddress = e.IpAddress,
                PortNumber = e.PortNumber,
                Model = e.Model,
                SerialNumber = e.SerialNumber,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetFingerprintDevicesResponse
            {
                FingerprintDevices = fingerprintDevicesList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetFingerprintDevicesForDropDownResponse> GetForDropDown(GetFingerprintDevicesCriteria criteria)
        {
            criteria.IsActive = true;
            var fingerprintDeviceRepository = repositoryManager.FingerprintDeviceRepository;
            var query = fingerprintDeviceRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = fingerprintDeviceRepository.OrderBy(query, nameof(FingerprintDevice.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var fingerprintDevicesList = await queryPaged.Select(e => new BaseGetForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetFingerprintDevicesForDropDownResponse
            {
                FingerprintDevices = fingerprintDevicesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetFingerprintDeviceInfoResponseModel> GetInfo(int fingerprintDeviceId)
        {
            var fingerprintDevice = await repositoryManager.FingerprintDeviceRepository.Get(e => e.Id == fingerprintDeviceId && !e.IsDeleted)
                .Select(e => new GetFingerprintDeviceInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IpAddress = e.IpAddress,
                    PortNumber = e.PortNumber,
                    SerialNumber = e.SerialNumber,
                    Model = e.Model,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceNotFound);

            return fingerprintDevice;
        }
        public async Task<GetFingerprintDeviceByIdResponseModel> GetById(int fingerprintDeviceId)
        {
            var fingerprintDevice = await repositoryManager.FingerprintDeviceRepository.Get(e => e.Id == fingerprintDeviceId && !e.IsDeleted)
                .Select(e => new GetFingerprintDeviceByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IpAddress = e.IpAddress,
                    PortNumber = e.PortNumber,
                    SerialNumber = e.SerialNumber,
                    Model = e.Model,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceNotFound);

            return fingerprintDevice;

        }
        public async Task<bool> Delete(int fingerprintDeviced)
        {
            var fingerprintDevice = await repositoryManager.FingerprintDeviceRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == fingerprintDeviced) ??
                throw new BusinessValidationException(LeillaKeys.SorryFingerprintDeviceNotFound);
            fingerprintDevice.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

