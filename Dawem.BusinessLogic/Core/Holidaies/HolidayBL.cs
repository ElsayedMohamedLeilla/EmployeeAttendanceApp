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
using NodaTime;
using NodaTime.Calendars;
using System.Collections.Immutable;
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
                getholiday.StartYear = null;
                getholiday.EndYear = null;
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
                StartDate = e.GetStartDateAsString(criteria.Year ?? DateTime.UtcNow.Year),
                EndDate = e.GetEndDateAsString(criteria.Year ?? DateTime.UtcNow.Year)
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
                    StartDate = e.GetStartDateAsString(null),
                    EndDate = e.GetEndDateAsString(null)

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);

            return holiday;
        }
        public async Task<GetHolidayByIdResponseDTO> GetById(int holidayId)
        {
            HijriCalendar hijriCalendar = new HijriCalendar();
            int currentHijriYear = hijriCalendar.GetYear(DateTime.UtcNow);
            var holiday = await repositoryManager.HolidayRepository.Get(e => e.Id == holidayId && !e.IsDeleted)
                .Select(e => new GetHolidayByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                    DateType = e.DateType,
                    Notes = e.Notes,
                    StartDate = e.DateType == DateType.Hijri ? e.GetHijriDate(currentHijriYear, e.StartMonth, e.StartDay) : new LocalDate(e.StartYear ?? DateTime.UtcNow.Year, e.StartMonth, e.StartDay),
                    EndDate = e.DateType == DateType.Hijri ? e.GetHijriDate(currentHijriYear, e.EndMonth, e.EndDay) : new LocalDate(e.EndYear ?? DateTime.UtcNow.Year, e.EndMonth, e.EndDay),

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

        public async Task<GetHolidaiesInformationsResponseDTO> GetHolidaiesInformation()
        {
            var leapYearPattern = IslamicLeapYearPattern.Base15;
            var epoch = IslamicEpoch.Astronomical;
            var holidayRepository = repositoryManager.HolidayRepository;
            var allHolidays = await holidayRepository
                .Get(holiday => !holiday.IsDeleted && holiday.CompanyId == requestInfo.CompanyId)
                .ToListAsync();

            var totalHolidayCount = allHolidays.Count;

            // Calculate Gregorian date counts
            var (upcomingGregorianCount, pastGregorianCount) = CalculateGregorianCounts(allHolidays.Where(h => h.DateType == DateType.Gregorian).ToList());

            // Calculate Hijri date counts
            var (upcomingHijriCount, pastHijriCount) = CalculateHijriCounts(allHolidays.Where(h => h.DateType == DateType.Hijri).ToList(), leapYearPattern, epoch);

            return new GetHolidaiesInformationsResponseDTO
            {
                TotalHolidayCount = totalHolidayCount,
                UpcomingHolidaiesCount = upcomingGregorianCount + upcomingHijriCount,
                PastHolidaiesCount = pastGregorianCount + pastHijriCount
            };
        }



        #region calculate HolidayCount for Hijri and gerogian date
        private (int, int) CalculateGregorianCounts(List<Holiday> holidays)
        {
            var today = DateTime.Today;

            var upcomingCount = holidays
                .Count(h =>
                    h.StartYear > today.Year ||
                    (h.StartYear == today.Year && h.StartMonth > today.Month) ||
                    (h.StartYear == today.Year && h.StartMonth == today.Month && h.StartDay > today.Day));


            var pastCount = holidays
                .Count(h =>
                    h.EndYear < today.Year ||
                    (h.EndYear == today.Year && h.EndMonth < today.Month) ||
                    (h.EndYear == today.Year && h.EndMonth == today.Month && h.EndDay < today.Day));


            return (upcomingCount, pastCount);
        }

        private (int, int) CalculateHijriCounts(List<Holiday> holidays, IslamicLeapYearPattern leapYearPattern, IslamicEpoch epoch)
        {
            var todayHijri = LocalDate.FromDateTime(DateTime.Today, CalendarSystem.GetIslamicCalendar(leapYearPattern, epoch));

            var upcomingCount = holidays
                .Count(h =>
                    (h.StartMonth > todayHijri.Month ||
                    (h.StartMonth == todayHijri.Month && h.StartDay > todayHijri.Day)));

            var pastCount = holidays
                .Count(h =>
                    (h.EndMonth < todayHijri.Month ||
                    (h.EndMonth == todayHijri.Month && h.EndDay < todayHijri.Day)));

            return (upcomingCount, pastCount);
        }

        #endregion



        //get holiday for mobile
        public async Task<GetHolidayResponseForEmployeeDTO> GetForEmployee()
        {
            var holidayRepository = repositoryManager.HolidayRepository;
            var allHolidays = await holidayRepository
                .Get(holiday => !holiday.IsDeleted && holiday.CompanyId == requestInfo.CompanyId)
                .ToListAsync();
            var holidays = allHolidays.Select(h =>
            {
                var (startDay, status) = GetStartEndDate(new StartEndDateParametersDTO
                {
                    dateType = h.DateType,
                    endDay = h.EndDay,
                    endMonth = h.EndMonth,
                    endYear = h.EndYear ?? DateTime.UtcNow.Year,
                    startDay = h.StartDay,
                    startMonth = h.StartMonth,
                    startYear = h.StartYear ?? DateTime.UtcNow.Year
                });

                return new GetHolidayForGridForEmployeeDTO()
                {
                    Id = h.Id,
                    Code = h.Code,
                    DateType = h.DateType == 0 ? TranslationHelper.GetTranslation(AmgadKeys.Gregorian, requestInfo.Lang) : TranslationHelper.GetTranslation(AmgadKeys.Hijri, requestInfo.Lang),
                    IsActive = h.IsActive,
                    Name = h.Name,
                    Period = GetPeriodLabel(h.StartMonth, h.StartDay, h.EndDay, h.DateType),
                    StartDay = startDay,
                    Status = status
                };
            }).ToList();

            return new GetHolidayResponseForEmployeeDTO()
            {
                Holidays = holidays,
                TotalCount = holidays.Count()

            };
        }

        public string GetPeriodLabel(int month, int startDay, int endDay, DateType dateType)
        {
            if (startDay != endDay)
                return startDay + " : " + endDay + " " + GetMonthName(month, dateType);
            else
                return startDay + " " + GetMonthName(month, dateType);

        }

        private string GetMonthName(int month, DateType dateType)
        {
            string monthName = "";

            if (dateType == DateType.Gregorian)
            {

                monthName = month switch
                {
                    1 => TranslationHelper.GetTranslation(AmgadKeys.January, requestInfo.Lang),
                    2 => TranslationHelper.GetTranslation(AmgadKeys.February, requestInfo.Lang),
                    3 => TranslationHelper.GetTranslation(AmgadKeys.March, requestInfo.Lang),
                    4 => TranslationHelper.GetTranslation(AmgadKeys.April, requestInfo.Lang),
                    5 => TranslationHelper.GetTranslation(AmgadKeys.May, requestInfo.Lang),
                    6 => TranslationHelper.GetTranslation(AmgadKeys.June, requestInfo.Lang),
                    7 => TranslationHelper.GetTranslation(AmgadKeys.July, requestInfo.Lang),
                    8 => TranslationHelper.GetTranslation(AmgadKeys.August, requestInfo.Lang),
                    9 => TranslationHelper.GetTranslation(AmgadKeys.September, requestInfo.Lang),
                    10 => TranslationHelper.GetTranslation(AmgadKeys.October, requestInfo.Lang),
                    11 => TranslationHelper.GetTranslation(AmgadKeys.November, requestInfo.Lang),
                    12 => TranslationHelper.GetTranslation(AmgadKeys.December, requestInfo.Lang),
                    _ => throw new ArgumentOutOfRangeException(nameof(month), TranslationHelper.GetTranslation(AmgadKeys.InvalidMonthNumber, requestInfo.Lang)),
                };
            }
            else
            {
                monthName = month switch
                {
                    1 => TranslationHelper.GetTranslation(AmgadKeys.Muharram, requestInfo.Lang),
                    2 => TranslationHelper.GetTranslation(AmgadKeys.Safar, requestInfo.Lang),
                    3 => TranslationHelper.GetTranslation(AmgadKeys.RabiAwwal, requestInfo.Lang),
                    4 => TranslationHelper.GetTranslation(AmgadKeys.RabialThani, requestInfo.Lang),
                    5 => TranslationHelper.GetTranslation(AmgadKeys.JumadaalAwwal, requestInfo.Lang),
                    6 => TranslationHelper.GetTranslation(AmgadKeys.JumadaalThani, requestInfo.Lang),
                    7 => TranslationHelper.GetTranslation(AmgadKeys.Rajab, requestInfo.Lang),
                    8 => TranslationHelper.GetTranslation(AmgadKeys.Shaaban, requestInfo.Lang),
                    9 => TranslationHelper.GetTranslation(AmgadKeys.Ramadan, requestInfo.Lang),
                    10 => TranslationHelper.GetTranslation(AmgadKeys.Shawwal, requestInfo.Lang),
                    11 => TranslationHelper.GetTranslation(AmgadKeys.DhualQidah, requestInfo.Lang),
                    12 => TranslationHelper.GetTranslation(AmgadKeys.DhualHijjah, requestInfo.Lang),
                    _ => throw new ArgumentOutOfRangeException(nameof(month), TranslationHelper.GetTranslation(AmgadKeys.InvalidMonthNumber, requestInfo.Lang)),
                };
            }
            return monthName;
        }

        private (string, HolidayStatus) GetStartEndDate(StartEndDateParametersDTO model)
        {
            string dayOfWeek = "";
            if (model.dateType == 0)  //Gregorian Date
            {
                //var getTimeZoneId =  repositoryManager.CompanyRepository
                // .Get(c => !c.IsDeleted && c.Id == requestInfo.CompanyId)
                // .Select(c => c.Country.TimeZoneId)
                // .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryTimeZoneNotFound);

                var clientLocalDateTime = DateTime.UtcNow;
                DateTime startDate = new(model.startYear, model.startMonth, model.startDay);
                DateTime endDate = new(model.endYear, model.endMonth, model.endDay);
                if (clientLocalDateTime < startDate) // will start
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)startDate.DayOfWeek).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.WillStart, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.WillStart);
                }
                else if (clientLocalDateTime < endDate && clientLocalDateTime > startDate) //starte
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)startDate.DayOfWeek).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.Started, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.Started);
                }
                else if (clientLocalDateTime > endDate) //ended 
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)endDate.DayOfWeek).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.Ended, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.Ended);
                }
                else
                {
                    return ("", HolidayStatus.NoSet);
                }
            }
            else //hijri Date
            {
                CalendarSystem hijriCalendar = CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.Base15, IslamicEpoch.Astronomical);
                LocalDate today = SystemClock.Instance.GetCurrentInstant().InUtc().Date.WithCalendar(hijriCalendar);
                LocalDate startDate = new LocalDate(today.Year, model.startMonth, model.startDay, hijriCalendar);
                LocalDate endDate = new LocalDate(today.Year, model.endMonth, model.endDay, hijriCalendar);
                if (today < startDate) // will start
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)startDate.DayOfWeek).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.WillStart, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.WillStart);
                }
                else if (today < endDate && today > startDate) //started
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)startDate.DayOfWeek).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.Started, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.Started);
                }
                else if (today > endDate) //ended 
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)endDate.DayOfWeek).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.Ended, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.Ended);
                }
                else
                {
                    return ("", HolidayStatus.NoSet);
                }
            }

        }












    }
}
