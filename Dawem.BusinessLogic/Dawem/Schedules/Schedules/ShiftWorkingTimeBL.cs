﻿using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Schedules.ShiftWorkingTime;
using Dawem.Contract.BusinessValidation.Dawem.Schedules.ShiftWorkingTimes;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.Models.Response.Dawem.Schedules.ShiftWorkingTimes;
using Dawem.Translations;
using Dawem.Validation.FluentValidation;
using Dawem.Validation.FluentValidation.Dawem.Schedules.ShiftWorkingTimes;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Schedules.Schedules
{
    public class ShiftWorkingTimeBL : IShiftWorkingTimeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IShiftWorkingTimeBLValidation shiftWorkingTimeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly INotificationHandleBL notificationHandleBL;
        public ShiftWorkingTimeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper, INotificationHandleBL _notificationHandleBL,
        RequestInfo _requestHeaderContext,
        IShiftWorkingTimeBLValidation _shiftWorkingTimeBLValidation)
        {
            unitOfWork = _unitOfWork;
            notificationHandleBL = _notificationHandleBL;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            shiftWorkingTimeBLValidation = _shiftWorkingTimeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateShiftWorkingTimeModelDTO model)
        {
            #region Model Validation

            var createShiftWorkingTimeModel = new CreateShiftWorkingTimeModelValidator();
            var createShiftWorkingTimeModelResult = createShiftWorkingTimeModel.Validate(model);
            if (!createShiftWorkingTimeModelResult.IsValid)
            {
                var error = createShiftWorkingTimeModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await shiftWorkingTimeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert ShiftWorkingTime

            #region Set ShiftWorkingTime code
            var getNextCode = await repositoryManager.ShiftWorkingTimeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var shiftWorkingTime = mapper.Map<ShiftWorkingTime>(model);
            shiftWorkingTime.CompanyId = requestInfo.CompanyId;
            shiftWorkingTime.AddUserId = requestInfo.UserId;
            shiftWorkingTime.Code = getNextCode;
            shiftWorkingTime.IsTwoDaysShift = TimeHelper.IsTwoDaysShift(shiftWorkingTime.CheckInTime, shiftWorkingTime.CheckOutTime);
            repositoryManager.ShiftWorkingTimeRepository.Insert(shiftWorkingTime);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response
            await unitOfWork.CommitAsync();
            return shiftWorkingTime.Id;
            #endregion

        }
        public async Task<bool> Update(UpdateShiftWorkingTimeModelDTO model)
        {
            #region Model Validation

            var updateShiftWorkingTimeModelValidator = new UpdateShiftWorkingTimeModelValidator();
            var updateShiftWorkingTimeModelValidatorResult = updateShiftWorkingTimeModelValidator.Validate(model);
            if (!updateShiftWorkingTimeModelValidatorResult.IsValid)
            {
                var error = updateShiftWorkingTimeModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await shiftWorkingTimeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update ShiftWorkingTime

            var getShiftWorkingTime = await repositoryManager.
                ShiftWorkingTimeRepository.
                GetEntityByConditionWithTrackingAsync(s => !s.IsDeleted &&
                s.CompanyId == requestInfo.CompanyId && s.Id == model.Id);

            var isWorkingHoursOrAllowedMinutesChanged = false;

            if (getShiftWorkingTime.CheckInTime != model.CheckInTime ||
                getShiftWorkingTime.CheckOutTime != model.CheckOutTime ||
                getShiftWorkingTime.AllowedMinutes != model.AllowedMinutes)
            {
                isWorkingHoursOrAllowedMinutesChanged = true;
            }

            getShiftWorkingTime.Name = model.Name;
            getShiftWorkingTime.AllowedMinutes = model.AllowedMinutes;
            getShiftWorkingTime.CheckInTime = model.CheckInTime;
            getShiftWorkingTime.CheckOutTime = model.CheckOutTime;
            getShiftWorkingTime.TimePeriod = model.TimePeriod;
            getShiftWorkingTime.IsActive = model.IsActive;
            getShiftWorkingTime.ModifiedDate = DateTime.UtcNow;
            getShiftWorkingTime.ModifyUserId = requestInfo.UserId;
            getShiftWorkingTime.IsTwoDaysShift = TimeHelper.IsTwoDaysShift(model.CheckInTime, model.CheckOutTime);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Notifications

            if (isWorkingHoursOrAllowedMinutesChanged)
            {
                var employees = await repositoryManager.EmployeeRepository.
                    Get(e => !e.IsDeleted && e.IsActive && e.CompanyId == requestInfo.CompanyId &&
                    e.Schedule.ScheduleDays.Any(sd => !sd.IsDeleted && sd.ShiftId == model.Id)).
                    Select(e => new
                    {
                        e.Id,
                        NotificationUsers = e.Users != null ? e.Users.Where(u => !u.IsDeleted).
                        Select(u => new NotificationUserModel
                        {
                            Id = u.Id,
                            Email = u.Email,
                            UserTokens = u.NotificationUsers.
                            Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.
                            Any(f => !f.IsDeleted)).
                            SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => !f.IsDeleted).
                            Select(f => new NotificationUserTokenModel
                            {
                                ApplicationType = f.DeviceType,
                                Token = f.FCMToken
                            })).ToList()
                        }).ToList() : null
                    }).ToListAsync();

                var employeeIds = employees.Select(e => e.Id).ToList();
                var shiftName = model.Name;

                var notificationUsers = employees.Where(e => e.NotificationUsers != null).
                    SelectMany(e => e.NotificationUsers).
                    Distinct().
                    ToList();

                #region Handle Notification Description

                var notificationDescriptions = new List<NotificationDescriptionModel>();

                var getActiveLanguages = await repositoryManager.LanguageRepository.Get(l => !l.IsDeleted && l.IsActive).
                       Select(l => new ActiveLanguageModel
                       {
                           Id = l.Id,
                           ISO2 = l.ISO2
                       }).ToListAsync();

                foreach (var language in getActiveLanguages)
                {
                    notificationDescriptions.Add(new NotificationDescriptionModel
                    {
                        LanguageIso2 = language.ISO2,
                        Description = TranslationHelper.GetTranslation(LeillaKeys.ThereIsNewUpdateInWorkingHoursInOneShiftOfYourScheduleShifts, language.ISO2) +
                        LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.ShiftName, language.ISO2) +
                            LeillaKeys.ColonsThenSpace +
                            shiftName +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.CheckInTime, language.ISO2) +
                            LeillaKeys.ColonsThenSpace + new DateTime(model.CheckInTime.Ticks).ToString("hh:mm") + LeillaKeys.Space +
                            TranslationHelper.GetTranslation(new DateTime(model.CheckInTime.Ticks).ToString("tt"), requestInfo.Lang) +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.CheckOutTime, language.ISO2) +
                            LeillaKeys.ColonsThenSpace + new DateTime(model.CheckOutTime.Ticks).ToString("hh:mm") + LeillaKeys.Space +
                            TranslationHelper.GetTranslation(new DateTime(model.CheckOutTime.Ticks).ToString("tt"), requestInfo.Lang) +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.AllowedMinutes, language.ISO2) +
                            LeillaKeys.ColonsThenSpace + model.AllowedMinutes + LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.Minute, requestInfo.Lang)
                    });
                }

                #endregion

                var handleNotificationModel = new HandleNotificationModel
                {
                    NotificationUsers = notificationUsers,
                    EmployeeIds = employeeIds,
                    NotificationType = NotificationType.NewChangeInSchedule,
                    NotificationStatus = NotificationStatus.Info,
                    Priority = NotificationPriority.Medium,
                    NotificationDescriptions = notificationDescriptions,
                    ActiveLanguages = getActiveLanguages
                };

                await notificationHandleBL.HandleNotifications(handleNotificationModel);
            }

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetShiftWorkingTimeResponseDTO> Get(GetShiftWorkingTimesCriteria criteria)
        {
            #region Model Validation

            var getValidator = new GetGenaricValidator(); // validate on pageining and all common validation 
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            var ShiftWorkingTimeRepository = repositoryManager.ShiftWorkingTimeRepository;
            var query = ShiftWorkingTimeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ShiftWorkingTimeRepository.OrderBy(query, nameof(ShiftWorkingTime.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var shiftWorkingTimesList = await queryPaged.Select(shift => new GetShiftWorkingTimeResponseModelDTO
            {
                Id = shift.Id,
                Code = shift.Code,
                Name = shift.Name,
                CheckInTime = shift.CheckInTime,
                CheckOutTime = shift.CheckOutTime,
                AllowedMinutes = shift.AllowedMinutes,
                TimePeriod = shift.TimePeriod,
                IsActive = shift.IsActive,
                EmployeesCount = shift.Company.Schedules
                .Where(s => !s.IsDeleted && s.Employees != null && s.ScheduleDays != null && 
                s.ScheduleDays.Any(sd => !sd.IsDeleted && sd.ShiftId == shift.Id))
                .SelectMany(s => s.Employees)
                .Count()
            }).ToListAsync();

            return new GetShiftWorkingTimeResponseDTO
            {
                ShiftWorkingTimes = shiftWorkingTimesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetShiftWorkingTimeDropDownResponseDTO> GetForDropDown(GetShiftWorkingTimesCriteria criteria)
        {
            #region Model Validation

            var getValidator = new GetGenaricValidator();
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            criteria.IsActive = true;
            var ShiftWorkingTimeRepository = repositoryManager.ShiftWorkingTimeRepository;
            var query = ShiftWorkingTimeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ShiftWorkingTimeRepository.OrderBy(query, nameof(ShiftWorkingTime.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var ShiftWorkingTimesList = await queryPaged.Select(e => new GetShiftWorkingTimeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetShiftWorkingTimeDropDownResponseDTO
            {
                ShiftWorkingTimes = ShiftWorkingTimesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetShiftWorkingTimeInfoResponseDTO> GetInfo(int ShiftWorkingTimeId)
        {
            var ShiftWorkingTime = await repositoryManager.ShiftWorkingTimeRepository.Get(e => e.Id == ShiftWorkingTimeId && !e.IsDeleted)
                .Select(e => new GetShiftWorkingTimeInfoResponseDTO
                {
                    Code = e.Code,
                    Name = e.Name,
                    CheckInTime = e.CheckInTime,
                    CheckOutTime = e.CheckOutTime,
                    AllowedMinutes = e.AllowedMinutes,
                    TimePeriod = e.TimePeriod,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryShiftWorkingTimeNotFound);

            return ShiftWorkingTime;
        }
        public async Task<GetShiftWorkingTimeByIdResponseDTO> GetById(int ShiftWorkingTimeId)
        {
            var ShiftWorkingTime = await repositoryManager.ShiftWorkingTimeRepository.Get(e => e.Id == ShiftWorkingTimeId && !e.IsDeleted)
                .Select(e => new GetShiftWorkingTimeByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    CheckInTime = e.CheckInTime,
                    CheckOutTime = e.CheckOutTime,
                    AllowedMinutes = e.AllowedMinutes,
                    TimePeriod = e.TimePeriod,
                    IsActive = e.IsActive,

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryShiftWorkingTimeNotFound);

            return ShiftWorkingTime;

        }
        public async Task<bool> Delete(int ShiftWorkingTimeId)
        {
            var ShiftWorkingTime = await repositoryManager.ShiftWorkingTimeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == ShiftWorkingTimeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryShiftWorkingTimeNotFound);
            ShiftWorkingTime.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Enable(int ShiftWorkingTimeId)
        {
            var employee = await repositoryManager.ShiftWorkingTimeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == ShiftWorkingTimeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryShiftWorkingTimeNotFound);
            employee.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var ShiftWorkingTime = await repositoryManager.ShiftWorkingTimeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryShiftWorkingTimeNotFound);
            ShiftWorkingTime.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetShiftWorkingTimesInformationsResponseDTO> GetShiftWorkingTimesInformations()
        {
            var shiftWorkingTimeRepository = repositoryManager.ShiftWorkingTimeRepository;
            var query = shiftWorkingTimeRepository.Get(shiftWorkingTime => shiftWorkingTime.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetShiftWorkingTimesInformationsResponseDTO
            {
                TotalCount = await query.CountAsync(),
                ActiveCount = await query.Where(shiftWorkingTime => !shiftWorkingTime.IsDeleted && shiftWorkingTime.IsActive).CountAsync(),
                NotActiveCount = await query.Where(shiftWorkingTime => !shiftWorkingTime.IsDeleted && !shiftWorkingTime.IsActive).CountAsync(),
                DeletedCount = await query.Where(shiftWorkingTime => shiftWorkingTime.IsDeleted).CountAsync()
            };

            #endregion
        }

    }
}
