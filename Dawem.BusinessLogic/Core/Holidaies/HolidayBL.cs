using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Holidaies;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.Holidaies;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Dawem.BusinessLogic.Core.holidays
{
    public class HolidayBL : IHolidayBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IHolidayBLValidation HolidayBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public HolidayBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IHolidayBLValidation _holidayBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            HolidayBLValidation = _holidayBLValidation;
            mapper = _mapper;
        }

        public async Task<int> Create(CreateHolidayDTO model)
        {
            model.AssignStartEndDayMonthYear(); // to assign startDay EndDay Year From Start and End Date
            #region Business Validation
            await HolidayBLValidation.CreateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Insert holiday

            #region Set holiday code

            var getNextCode = await repositoryManager.HolidayRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var holiday = mapper.Map<Holiday>(model);
            holiday.CompanyId = requestInfo.CompanyId;
            holiday.AddUserId = requestInfo.UserId;
            holiday.Code = getNextCode;
            repositoryManager.HolidayRepository.Insert(holiday);
            await unitOfWork.SaveAsync();
            #endregion
            #region Handle Response
            await unitOfWork.CommitAsync();
            return holiday.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateHolidayDTO model)
        {
            model.AssignStartEndDayMonthYear(); // to assign startDay EndDay Year From Start and End Date

            #region Business Validation
            await HolidayBLValidation.UpdateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();
            #region Update Holiday
            var getholiday = await repositoryManager.HolidayRepository.GetByIdAsync(model.Id);
            getholiday.Name = model.Name;
            getholiday.IsActive = model.IsActive;
            getholiday.ModifiedDate = DateTime.Now;
            getholiday.ModifyUserId = requestInfo.UserId;
            getholiday.DateType = model.DateType;
            getholiday.StartDay = model.StartDay;
            getholiday.EndDay = model.EndDay;
            getholiday.StartMonth = model.StartMonth;
            getholiday.EndMonth = model.EndMonth;
            if (model.DateType == DateType.Hijri) //ignore year in Hijri Data
            {
                getholiday.StartYear = 0;
                getholiday.EndYear = 0;
            }
            else
            {
                getholiday.StartYear = model.StartYear;
                getholiday.EndYear = model.EndYear;
            }
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetHolidayResponseDTO> Get(GetHolidayCriteria criteria)
        {
            var holidayRepository = repositoryManager.HolidayRepository;
            var query = holidayRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = holidayRepository.OrderBy(query, nameof(Holiday.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var holidaysList = await queryPaged.Select(e => new GetHolidayForGridDTO
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
                DateType = e.DateType == 0 ? TranslationHelper.GetTranslation(AmgadKeys.Gregorian, requestInfo.Lang) : TranslationHelper.GetTranslation(AmgadKeys.Hijri, requestInfo.Lang),
                Notes = e.Notes,
                StartDate = e.GetStartDate(),
                EndDate = e.GetEndDate()
            }).ToListAsync();

            return new GetHolidayResponseDTO
            {
                Holidaies = holidaysList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetHolidayDropDownResponseDTO> GetForDropDown(GetHolidayCriteria criteria)
        {
            criteria.IsActive = true;
            var holidayRepository = repositoryManager.HolidayRepository;
            var query = holidayRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = holidayRepository.OrderBy(query, nameof(Holiday.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var holidaysList = await queryPaged.Select(e => new GetHolidayForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetHolidayDropDownResponseDTO
            {
                Holidaies = holidaysList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetHolidayInfoResponseDTO> GetInfo(int holidayId)
        {
            var holiday = await repositoryManager.HolidayRepository.Get(e => e.Id == holidayId && !e.IsDeleted)
                .Select(e => new GetHolidayInfoResponseDTO
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                    DateType = e.DateType == 0 ? TranslationHelper.GetTranslation(AmgadKeys.Gregorian, requestInfo.Lang) : TranslationHelper.GetTranslation(AmgadKeys.Hijri, requestInfo.Lang),
                    Notes = e.Notes,
                    StartDate = e.GetStartDate(),
                    EndDate = e.GetEndDate()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);

            return holiday;
        }
        public async Task<GetHolidayByIdResponseDTO> GetById(int holidayId)
        {
            var holiday = await repositoryManager.HolidayRepository.Get(e => e.Id == holidayId && !e.IsDeleted)

                .Select(e => new GetHolidayByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                    DateType = e.DateType,
                    StartDate = e.DateType ==
                    DateType.Hijri ? new HijriCalendar().ToDateTime(DateTime.Now.Year, e.StartMonth, e.StartDay, 0, 0, 0, 0).Date
                     : new DateTime(e.StartYear ?? DateTime.Now.Year, e.StartMonth, e.StartDay).Date,
                    EndDate = e.DateType == DateType.Hijri
                     ? new HijriCalendar().ToDateTime(DateTime.Now.Year, e.EndMonth, e.EndDay, 0, 0, 0, 0).Date
                     : new DateTime(e.EndYear ?? DateTime.Now.Year, e.EndMonth, e.EndDay).Date


                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);

            return holiday;

        }
        public async Task<bool> Delete(int holidayId)
        {
            var holiday = await repositoryManager.HolidayRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == holidayId) ??
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);
            holiday.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Enable(int holidayId)
        {
            var holiday = await repositoryManager.HolidayRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == holidayId) ??
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);
            holiday.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var holiday = await repositoryManager.HolidayRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);
            holiday.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }

    }
}
