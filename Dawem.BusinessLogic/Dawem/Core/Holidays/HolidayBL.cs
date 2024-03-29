using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.Holidays;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.Holidays;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Calendars;
using NodaTime.Extensions;
using System.Globalization;

namespace Dawem.BusinessLogic.Dawem.Core.Holidays
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
            model.JustifyStartEndDate();
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
            model.JustifyStartEndDate();

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
            getholiday.StartDate = model.StartDate;
            getholiday.EndDate = model.EndDate;
            getholiday.Notes = model.Notes;
            getholiday.IsSpecifiedByYear = model.IsSpecifiedByYear;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response
            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }


        public async Task<GetHolidayResponseDTO> Get(GetHolidayCriteria criteria)
        {
            //LocalDateTime CurrentHijri = requestInfo.LocalHijriDateTime;
            var holidayRepository = repositoryManager.HolidayRepository;
            var query = holidayRepository.GetAsQueryable(criteria);
            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = holidayRepository.OrderBy(query, nameof(Holiday.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
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
                StartDate = e.CreateStartEndDate().Item1,
                EndDate = e.CreateStartEndDate().Item2
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

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

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
                    StartDate = e.CreateStartEndDate().Item1,
                    EndDate = e.CreateStartEndDate().Item2

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);

            return holiday;
        }
        public async Task<GetHolidayByIdResponseDTO> GetById(int holidayId)
        {
            HijriCalendar hijriCalendar = new HijriCalendar();
            int currentHijriYear = hijriCalendar.GetYear(DateTime.UtcNow);
            var holiday = await repositoryManager.HolidayRepository.Get(e => e.Id == holidayId && !e.IsDeleted)
                .Select(e =>

                new GetHolidayByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                    DateType = e.DateType,
                    Notes = e.Notes,
                    StartDate = DateTime.Parse(e.CreateStartEndDate().Item1).Date,
                    EndDate = DateTime.Parse(e.CreateStartEndDate().Item2).Date

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
            allHolidays = JsutifyStartEndDate(allHolidays);
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



        #region calculate HolidayCout for Hijri and gerogian date
        private (int, int) CalculateGregorianCounts(List<Holiday> holidays)
        {
            var today = DateTime.Today;

            var upcomingCount = holidays
                .Count(h =>
                    h.StartDate.Year > today.Year ||
                    h.StartDate.Year == today.Year && h.StartDate.Month > today.Month ||
                    h.StartDate.Year == today.Year && h.StartDate.Month == today.Month && h.StartDate.Day > today.Day);


            var pastCount = holidays
                .Count(h =>
                    h.EndDate.Year < today.Year ||
                    h.EndDate.Year == today.Year && h.EndDate.Month < today.Month ||
                    h.EndDate.Year == today.Year && h.EndDate.Month == today.Month && h.EndDate.Day < today.Day);


            return (upcomingCount, pastCount);
        }

        private (int, int) CalculateHijriCounts(List<Holiday> holidays, IslamicLeapYearPattern leapYearPattern, IslamicEpoch epoch)
        {
            var todayHijri = LocalDate.FromDateTime(DateTime.Today, CalendarSystem.GetIslamicCalendar(leapYearPattern, epoch));

            var upcomingCount = holidays
                .Count(h =>
                    h.StartDate.Month > todayHijri.Month ||
                    h.StartDate.Month == todayHijri.Month && h.StartDate.Day > todayHijri.Day);

            var pastCount = holidays
                .Count(h =>
                    h.EndDate.Month < todayHijri.Month ||
                    h.EndDate.Month == todayHijri.Month && h.EndDate.Day < todayHijri.Day);

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
                var (startStatus, status) = GetStartEndStatus(new StartEndDateParametersDTO
                {
                    dateType = h.DateType,
                    StartDate = h.IsSpecifiedByYear ? h.StartDate : new DateTime(DateTime.UtcNow.Year, h.StartDate.Month, h.StartDate.Day),
                    EndDate = h.IsSpecifiedByYear ? h.EndDate : new DateTime(DateTime.UtcNow.Year, h.EndDate.Month, h.EndDate.Day),
                    IsSpecificByYear = h.IsSpecifiedByYear
                });

                return new GetHolidayForGridForEmployeeDTO()
                {
                    Id = h.Id,
                    Code = h.Code,
                    DateType = h.DateType == 0 ? TranslationHelper.GetTranslation(AmgadKeys.Gregorian, requestInfo.Lang) : TranslationHelper.GetTranslation(AmgadKeys.Hijri, requestInfo.Lang),
                    IsActive = h.IsActive,
                    Name = h.Name,
                    Period = GetPeriodLabel(h.StartDate.Month, h.StartDate.Day, h.EndDate.Day, h.DateType),
                    StartStatus = startStatus,
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
            string monthName;

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

        private (string, HolidayStatus) GetStartEndStatus(StartEndDateParametersDTO model)
        {
            string dayOfWeek;
            if (model.dateType == 0)  //Gregorian Date
            {


                var clientLocalDateTime = requestInfo.LocalDateTime;
                var (startDate, endDate) = JustifyStartEndDate(model.IsSpecificByYear, model.dateType, model.StartDate, model.EndDate);
                if (clientLocalDateTime < startDate) // will start
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)startDate.DayOfWeek).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.WillStart, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.WillStart);
                }
                else if (clientLocalDateTime < endDate && clientLocalDateTime > startDate) //start
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
                LocalDate startDate = new LocalDate(today.Year, model.StartDate.Month, model.StartDate.Day, hijriCalendar);
                LocalDate endDate = new LocalDate(today.Year, model.EndDate.Month, model.EndDate.Day, hijriCalendar);
                if (today < startDate) // will start
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)startDate.DayOfWeek.ToDayOfWeek()).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.WillStart, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.WillStart);
                }
                else if (today < endDate && today > startDate) //started
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)startDate.DayOfWeek.ToDayOfWeek()).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.Started, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.Started);
                }
                else if (today > endDate) //ended 
                {
                    dayOfWeek = TranslationHelper.GetTranslation(((WeekDay)endDate.DayOfWeek.ToDayOfWeek()).ToString(), requestInfo.Lang);
                    return (TranslationHelper.GetTranslation(AmgadKeys.Ended, requestInfo.Lang) + " " + dayOfWeek, HolidayStatus.Ended);
                }
                else
                {
                    return ("", HolidayStatus.NoSet);
                }
            }

        }


        public (DateTime, DateTime) JustifyStartEndDate(bool IsSpecifiedByYear, DateType dateType, DateTime startDate, DateTime endDate)
        {

            if (!IsSpecifiedByYear)
            {
                if (dateType == DateType.Hijri)
                {
                    HijriCalendar hijriCalendar = new HijriCalendar();
                    int currentHijriYear = hijriCalendar.GetYear(DateTime.UtcNow);
                    DateTime start = new DateTime(currentHijriYear, startDate.Month, startDate.Day);
                    DateTime end = new DateTime(currentHijriYear, endDate.Month, endDate.Day);
                    return (startDate, endDate);
                }
                else
                {
                    DateTime today = DateTime.UtcNow;
                    DateTime start = new DateTime(today.Year, startDate.Month, startDate.Day);
                    DateTime end = new DateTime(today.Year, endDate.Month, endDate.Day);
                    return (startDate, endDate);
                }

            }
            else
            {
                return (startDate, endDate);
            }
        }

        private List<Holiday> JsutifyStartEndDate(List<Holiday> holidays)
        {
            for (int i = 0; i < holidays.Count; i++)
            {
                if (!holidays[i].IsSpecifiedByYear)
                {
                    if (holidays[i].DateType == DateType.Gregorian)
                    {
                        DateTime start = holidays[i].StartDate;
                        DateTime end = holidays[i].EndDate;
                        holidays[i].StartDate = new DateTime(requestInfo.LocalDateTime.Year, start.Month, start.Day);
                        holidays[i].EndDate = new DateTime(requestInfo.LocalDateTime.Year, end.Month, end.Day);

                    }
                    else
                    {
                        HijriCalendar hijriCalendar = new HijriCalendar();
                        int currentHijriYear = hijriCalendar.GetYear(DateTime.UtcNow);
                        DateTime start = holidays[i].StartDate;
                        DateTime end = holidays[i].EndDate;
                        holidays[i].StartDate = new DateTime(currentHijriYear, start.Month, start.Day);
                        holidays[i].EndDate = new DateTime(currentHijriYear, end.Month, end.Day);
                    }
                }

            }
            return holidays;
        }












    }
}
