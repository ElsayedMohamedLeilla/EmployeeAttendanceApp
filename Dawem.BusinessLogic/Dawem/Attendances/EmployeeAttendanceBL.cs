using ClosedXML.Excel;
using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Contract.BusinessValidation.Dawem.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Excel;
using Dawem.Models.Dtos.Dawem.Excel.EmployeeAttendances;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.Models.Response.Dawem.Dashboard;
using Dawem.Translations;
using Dawem.Validation.BusinessValidation.Dawem.ExcelValidations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace Dawem.BusinessLogic.Dawem.Attendances
{
    public class EmployeeAttendanceBL : IEmployeeAttendanceBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IEmployeeAttendanceBLValidation employeeAttendanceBLValidation;
        private readonly IRepositoryManager repositoryManager;
        public EmployeeAttendanceBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
           RequestInfo _requestHeaderContext,
           IEmployeeAttendanceBLValidation _employeeAttendanceBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            employeeAttendanceBLValidation = _employeeAttendanceBLValidation;
        }
        public async Task<FingerPrintType> CreateFingerPrint(FingerprintModel model)
        {
            #region Business Validation

            var validationResult = await employeeAttendanceBLValidation.FingerPrintValidation(model);

            #endregion

            await unitOfWork.CreateTransactionAsync();

            #region Hanlde FingerPrint

            var getEmployeeAttendance = await repositoryManager
                .EmployeeAttendanceRepository
                .GetWithTracking(e => !e.IsDeleted && e.EmployeeId == validationResult.EmployeeId
                && e.LocalDate.Date == validationResult.LocalDateTime.Date)
                .FirstOrDefaultAsync();

            //checkout or summon
            if (getEmployeeAttendance != null)
            {                
                repositoryManager.EmployeeAttendanceCheckRepository.Insert(new EmployeeAttendanceCheck
                {
                    EmployeeAttendanceId = getEmployeeAttendance.Id,
                    SummonId = validationResult.SummonId,
                    ZoneId = validationResult.ZoneId,
                    FingerPrintType = validationResult.FingerPrintType,
                    IsActive = true,
                    FingerPrintDate = requestInfo.LocalDateTime,
                    FingerPrintDateUTC = DateTime.UtcNow,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    IpAddress = requestInfo.RemoteIpAddress,
                    RecognitionWay = model.RecognitionWay == RecognitionWay.NotSet ?
                    RecognitionWay.FingerPrint : model.RecognitionWay,
                    FingerprintSource = FingerprintSource.MobileDevice
                });

                getEmployeeAttendance.FingerPrintStatus = AttendanceFingerPrintStatus.CheckInAndCheckOut;
                await unitOfWork.SaveAsync();

                #region Summon Log

                if (validationResult.FingerPrintType == FingerPrintType.Summon)
                {
                    var getSummonLog = await repositoryManager.SummonLogRepository
                        .GetEntityByConditionWithTrackingAsync(s => !s.IsDeleted && s.SummonId == validationResult.SummonId &&
                        s.EmployeeId == validationResult.EmployeeId);
                    if (getSummonLog != null)
                    {
                        getSummonLog.DoneSummon = true;
                        getSummonLog.DoneDate = validationResult.LocalDateTime;
                        await unitOfWork.SaveAsync();
                    }
                }

                #endregion
            }
            //checkin
            else
            {
                #region Insert Employee Attendance

                #region Set Employee Attendance code

                var getNextCode = await repositoryManager.EmployeeAttendanceRepository
                    .Get(e => e.CompanyId == requestInfo.CompanyId)
                    .Select(e => e.Code)
                    .DefaultIfEmpty()
                    .MaxAsync() + 1;

                #endregion

                var employeeAttendance = new EmployeeAttendance
                {
                    Code = getNextCode,
                    CompanyId = requestInfo.CompanyId,
                    ScheduleId = validationResult.ScheduleId,
                    ShiftId = validationResult.ShiftId,
                    ShiftCheckInTime = validationResult.ShiftCheckInTime,
                    ShiftCheckOutTime = validationResult.ShiftCheckOutTime,
                    IsTwoDaysShift = validationResult.IsTwoDaysShift,
                    AllowedMinutes = validationResult.AllowedMinutes,
                    AddedApplicationType = requestInfo.ApplicationType,
                    AddUserId = requestInfo.UserId,
                    LocalDate = validationResult.LocalDateTime,
                    EmployeeId = validationResult.EmployeeId,
                    IsActive = true,
                    FingerPrintStatus = AttendanceFingerPrintStatus.CheckIn,
                    EmployeeAttendanceChecks = new List<EmployeeAttendanceCheck> { new() {
                        FingerPrintType = validationResult.FingerPrintType,
                        IsActive = true,
                        ZoneId = validationResult.ZoneId,
                        FingerPrintDate = requestInfo.LocalDateTime,
                        FingerPrintDateUTC = DateTime.UtcNow,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        IpAddress = requestInfo.RemoteIpAddress,
                        RecognitionWay = model.RecognitionWay == RecognitionWay.NotSet ?
                        RecognitionWay.FingerPrint : model.RecognitionWay,
                        FingerprintSource = FingerprintSource.MobileDevice
                    } }
                };

                repositoryManager.EmployeeAttendanceRepository.Insert(employeeAttendance);

                #endregion
            }

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();

            return validationResult.FingerPrintType;

            #endregion
        }
        public async Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfo()
        {
            #region Business Validation
            var result = await employeeAttendanceBLValidation.GetCurrentFingerPrintInfoValidation();
            #endregion

            return result;
        }
        public async Task<List<GetEmployeeAttendancesResponseModel>> GetEmployeeAttendances(GetEmployeeAttendancesCriteria criteria)
        {
            var resonse = new List<GetEmployeeAttendancesResponseModel>();

            #region Business Validation

            var result = await employeeAttendanceBLValidation.GetEmployeeAttendancesValidation(criteria);

            #endregion

            var getEmployeeId = requestInfo?.User?.EmployeeId;

            var employeeAttendances = await repositoryManager.EmployeeAttendanceRepository
                .Get(a => !a.IsDeleted && a.EmployeeId == getEmployeeId
                && a.LocalDate.Date.Month == criteria.Month
                && a.LocalDate.Date.Year == criteria.Year)
                .Select(a => new EmployeeAttendance
                {
                    Id = a.Id,
                    LocalDate = a.LocalDate,
                    ShiftCheckInTime = a.ShiftCheckInTime,
                    ShiftCheckOutTime = a.ShiftCheckOutTime,
                    AllowedMinutes = a.AllowedMinutes,
                    EmployeeAttendanceChecks = a.EmployeeAttendanceChecks != null ?
                    a.EmployeeAttendanceChecks.Select(c => new EmployeeAttendanceCheck
                    {
                        Id = c.Id,
                        FingerPrintDate = c.FingerPrintDate,
                        FingerPrintType = c.FingerPrintType
                    }).ToList() : null
                }).ToListAsync();

            var allDatesInMonth = OthersHelper.AllDatesInMonth(criteria.Year, criteria.Month).Where(d => d.Date <= DateTime.UtcNow.Date).ToList();
            var maxDate = allDatesInMonth[allDatesInMonth.Count - 1];

            var employeePlans = await repositoryManager.SchedulePlanRepository.Get(s => !s.IsDeleted && s.DateFrom.Date <= maxDate.Date &&
                (s.SchedulePlanEmployee != null && s.SchedulePlanEmployee.EmployeeId == getEmployeeId ||
                s.SchedulePlanGroup != null && s.SchedulePlanGroup.Group.GroupEmployees != null && s.SchedulePlanGroup.Group.GroupEmployees.Any(g => g.EmployeeId == getEmployeeId) ||
                s.SchedulePlanDepartment != null && s.SchedulePlanDepartment.Department.Employees != null && s.SchedulePlanDepartment.Department.Employees.Any(g => g.Id == getEmployeeId)))
                .Select(s => new SchedulePlan
                {
                    DateFrom = s.DateFrom,
                    ScheduleId = s.ScheduleId
                }).ToListAsync();

            var employeePlansIds = employeePlans.Select(e => e.ScheduleId).ToList();

            var shifts = employeePlans != null ? await repositoryManager
                         .ScheduleDayRepository.Get(s => !s.IsDeleted && employeePlansIds.Contains(s.ScheduleId))
                         .Select(s => new
                         {
                             s.WeekDay,
                             s.ScheduleId,
                             s.ShiftId
                         }).ToListAsync() : null;

            var weekVacationDays = new List<DayAndWeekDayModel>();

            foreach (var date in allDatesInMonth)
            {
                var employeeAttendance = employeeAttendances
                        .FirstOrDefault(e => e.LocalDate.Date == date.Date);

                var checkInDateTime = employeeAttendance?.EmployeeAttendanceChecks?
                    .FirstOrDefault(c => c.FingerPrintType == FingerPrintType.CheckIn)?.FingerPrintDate;
                var checkOutDateTime = employeeAttendance?.EmployeeAttendanceChecks?
                    .Where(c => c.FingerPrintType == FingerPrintType.CheckOut)?
                    .OrderByDescending(c => c.Id)?.FirstOrDefault()?.FingerPrintDate;

                #region Check For Vacation

                var scheduleId = employeePlans.Where(s => s.DateFrom.Date <= date.Date)
                    .OrderByDescending(c => c.DateFrom.Date)?.FirstOrDefault()?.ScheduleId;

                int? shiftId = null;
                if (scheduleId != null)
                {
                    shiftId = shifts.FirstOrDefault(s => s.ScheduleId == scheduleId && s.WeekDay == (WeekDay)date.DayOfWeek)
                         .ShiftId;
                }

                var isScheduleVacationDay = false;

                #endregion

                if (scheduleId != null && shiftId == null)
                {
                    isScheduleVacationDay = true;
                    weekVacationDays.Add(new DayAndWeekDayModel()
                    {
                        Day = date.Day,
                        WeekDay = (WeekDay)date.DayOfWeek
                    });
                }

                if (!isScheduleVacationDay || (employeeAttendance != null && (checkInDateTime != null || checkOutDateTime != null)))
                {

                    var employeeAttendanceModel = new GetEmployeeAttendancesResponseModel
                    {
                        Attendance = new GetEmployeeAttendanceModel
                        {
                            Id = employeeAttendance?.Id,
                            Day = date.Day,
                            WeekDay = (WeekDay)date.DayOfWeek,
                            WeekDayName = TranslationHelper.GetTranslation(((WeekDay)date.DayOfWeek).ToString(), requestInfo.Lang),
                            CheckInTime = checkInDateTime != null ?
                            checkInDateTime.Value.ToString("HH:mm:ss") : null,
                            CheckOutTime = checkOutDateTime != null ?
                            checkOutDateTime.Value.ToString("HH:mm:ss") : null,
                            CheckInStatus = employeeAttendance != null && checkInDateTime != null ? (decimal)(checkInDateTime.Value.TimeOfDay -
                            employeeAttendance.ShiftCheckInTime).TotalMinutes > employeeAttendance.AllowedMinutes ? EmployeeAttendanceStatus.Warning : EmployeeAttendanceStatus.Success : EmployeeAttendanceStatus.Error,
                            CheckOutStatus = checkOutDateTime == null ? EmployeeAttendanceStatus.Error :
                            checkOutDateTime.Value.TimeOfDay < employeeAttendance.ShiftCheckOutTime ? EmployeeAttendanceStatus.Warning :
                            EmployeeAttendanceStatus.Success,
                            TotalTime = checkOutDateTime != null ?
                            TimeOnly.FromTimeSpan(checkOutDateTime.Value - checkInDateTime.Value).ToString("HH:mm:ss") : null,
                            Notes = isScheduleVacationDay ?
                            TranslationHelper.GetTranslation(LeillaKeys.WeekVacation, requestInfo.Lang) : null
                        }
                    };
                    employeeAttendanceModel.Attendance.AttendanceStatus =
                        employeeAttendanceModel.Attendance.CheckInStatus == EmployeeAttendanceStatus.Error ||
                        employeeAttendanceModel.Attendance.CheckOutStatus == EmployeeAttendanceStatus.Error ? EmployeeAttendanceStatus.Error :
                        employeeAttendanceModel.Attendance.CheckInStatus == EmployeeAttendanceStatus.Warning ||
                        employeeAttendanceModel.Attendance.CheckOutStatus == EmployeeAttendanceStatus.Warning ? EmployeeAttendanceStatus.Warning :
                        EmployeeAttendanceStatus.Success;

                    resonse.Add(employeeAttendanceModel);
                }

                if (date.DayOfWeek == (DayOfWeek)WeekDay.Friday && weekVacationDays.Count > 0)
                {
                    var allVacationsText = LeillaKeys.EmptyString;

                    for (int i = 0; i < weekVacationDays.Count; i++)
                    {
                        var item = weekVacationDays[i];
                        allVacationsText += item.Day + LeillaKeys.Space + TranslationHelper.GetTranslation(item.WeekDay.ToString(), requestInfo.Lang) +
                            (weekVacationDays.Count - i > 1 ? LeillaKeys.SpaceThenDashThenSpace : null);
                    }
                    resonse.Add(new GetEmployeeAttendancesResponseModel()
                    {
                        Attendance = null,
                        Informations = TranslationHelper.GetTranslation(LeillaKeys.EndOfWeekVacations, requestInfo.Lang) + allVacationsText
                    });

                    weekVacationDays = new List<DayAndWeekDayModel>();
                }
            }

            return resonse;
        }
        public async Task<GetEmployeeAttendancesResponseForWebDTO> GetEmployeeAttendancesForWebAdmin(GetEmployeeAttendancesForWebAdminCriteria criteria)
        {
            var employeeAttendanceRepository = repositoryManager.EmployeeAttendanceRepository;
            var query = employeeAttendanceRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeAttendanceRepository.OrderBy(query, nameof(EmployeeAttendance.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response



            #endregion

            var response = await queryPaged
               .Select(empAttendance => new GetEmployeeAttendancesResponseForWebAdminModelDTO
               {
                   Id = empAttendance.Id,
                   EmployeeNumber = empAttendance.Employee.EmployeeNumber,
                   EmployeeName = empAttendance.Employee.Name,
                   Date = empAttendance.LocalDate.Date,

                   CheckInDateTime =
                   empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn) != null ?
                     empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                    .Min(check => check.FingerPrintDate).ToString("dd-MM-yyyy hh:mm") + DateHelper.TranslateAmAndPm(empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                    .Min(check => check.FingerPrintDate).ToString("tt"), requestInfo.Lang) : null,

                   CheckOutDateTime =
                   empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut) != null ?
                     empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                    .Max(check => check.FingerPrintDate).ToString("dd-MM-yyyy hh:mm") + DateHelper.TranslateAmAndPm(empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                    .Max(check => check.FingerPrintDate).ToString("tt"), requestInfo.Lang) : null,

                   WayOfRecognition = GetWayOfRecognition(
                      empAttendance.EmployeeAttendanceChecks
                       .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                       .OrderBy(check => check.FingerPrintDate)
                       .Select(check => check.RecognitionWay)
                       .FirstOrDefault(),
                      empAttendance
                      .EmployeeAttendanceChecks
                       .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                       .OrderByDescending(check => check.FingerPrintDate)
                       .Select(check => check.RecognitionWay)
                       .FirstOrDefault(), requestInfo.Lang),

                   WorkingHours = (empAttendance.TotalWorkingHours ?? 0) + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                   Status = DetermineAttendanceStatus(empAttendance.ShiftCheckInTime, empAttendance.AllowedMinutes, empAttendance.EmployeeAttendanceChecks
                       .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                       .OrderBy(check => check.FingerPrintDate)
                       .Select(check => check.FingerPrintDate)
                       .FirstOrDefault(), requestInfo.Lang)

               }).ToListAsync();

            return new GetEmployeeAttendancesResponseForWebDTO
            {
                EmployeeAttendances = response,
                TotalCount = await query.CountAsync()
            };

        }
        public static string GetWayOfRecognition(RecognitionWay MinCheckinRecognitionWay, RecognitionWay MaxCheckOutRecognitionWay, string lang)
        {
            string checkInMethod = GetRecognitionMethodName(MinCheckinRecognitionWay, lang);
            string checkOutMethod = GetRecognitionMethodName(MaxCheckOutRecognitionWay, lang);

            if (checkInMethod == checkOutMethod)
            {
                return checkInMethod;
            }
            else
            {
                return checkInMethod + " / " + checkOutMethod;
            }
        }
        private static string GetRecognitionMethodName(RecognitionWay way, string lang)
        {
            return way switch
            {
                RecognitionWay.FaceRecognition => TranslationHelper.GetTranslation(AmgadKeys.FaceRecognition, lang),
                RecognitionWay.FingerPrint => TranslationHelper.GetTranslation(AmgadKeys.FingerPrint, lang),
                RecognitionWay.VoiceRecognition => TranslationHelper.GetTranslation(AmgadKeys.VoiceRecognition, lang),
                RecognitionWay.PinRecognition => TranslationHelper.GetTranslation(AmgadKeys.PinRecognition, lang),
                RecognitionWay.PaternRecognition => TranslationHelper.GetTranslation(AmgadKeys.PaternRecognition, lang),
                RecognitionWay.PasswordRecognition => TranslationHelper.GetTranslation(AmgadKeys.PasswordRecognition, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang),
            };
        }
        public static string DetermineAttendanceStatus(TimeSpan shiftCheckInTime, int allowedMinutes, DateTime chekInDateTime, string lang)
        {
            var newShiftCheckInTime = shiftCheckInTime + TimeSpan.FromMinutes(allowedMinutes);

            if (chekInDateTime.TimeOfDay < shiftCheckInTime)
            {
                return TranslationHelper.GetTranslation(AmgadKeys.Early, lang); ;
            }
            else if (chekInDateTime.TimeOfDay >= shiftCheckInTime && chekInDateTime.TimeOfDay <= newShiftCheckInTime)
            {
                return TranslationHelper.GetTranslation(AmgadKeys.OnTime, lang); ;
            }
            else if (chekInDateTime.TimeOfDay > newShiftCheckInTime)
            {
                return TranslationHelper.GetTranslation(AmgadKeys.Late, lang); ;
            }
            else
            {
                return TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang); ;
            }
        }
        public static string CalculateDelay(TimeOnly shiftCheckInTime, int allowedMinutes, DateTime actualCheckInTime)
        {
            DateTime scheduledCheckIn = actualCheckInTime.Date.Add(shiftCheckInTime.ToTimeSpan());
            DateTime scheduledCheckInAfterAddAllowedMinute = scheduledCheckIn.AddMinutes(allowedMinutes);
            // Calculate the time gap in minutes
            TimeSpan timeGap = actualCheckInTime - scheduledCheckInAfterAddAllowedMinute;
            // Ensure the time gap is non-negative
            TimeSpan nonNegativeDelay = TimeSpan.FromMinutes(Math.Max(timeGap.TotalMinutes, 0));
            // Format the non-negative time gap into HH:mm
            return nonNegativeDelay.ToString(@"hh\:mm");
        }
        public async Task<GetEmployeeAttendanceInfoDTO> GetEmployeeAttendancesInfo(int employeeAttendanceId)
        {
            var result = await repositoryManager.EmployeeAttendanceRepository.Get(s => !s.IsDeleted && s.Id == employeeAttendanceId)
                .Select(empAttendance => new GetEmployeeAttendanceInfoDTO
                {
                    EmployeeName = empAttendance.Employee.Name,
                    Date = empAttendance.LocalDate.Date,
                    CheckInDateTime =
                   empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn) != null ?
                     empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                    .Min(check => check.FingerPrintDate).ToString("dd-MM-yyyy hh:mm") + DateHelper.TranslateAmAndPm(empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                    .Min(check => check.FingerPrintDate).ToString("tt"), requestInfo.Lang) : null,

                    CheckOutDateTime =
                   empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut) != null ?
                     empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                    .Max(check => check.FingerPrintDate).ToString("dd-MM-yyyy hh:mm") + DateHelper.TranslateAmAndPm(empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                    .Max(check => check.FingerPrintDate).ToString("tt"), requestInfo.Lang) : null,

                    /*WayOfRecognition = GetWayOfRecognition(
                      empAttendance.EmployeeAttendanceChecks
                       .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                       .OrderBy(check => check.Time)
                       .Select(check => check.RecognitionWay)
                       .FirstOrDefault(),
                      empAttendance
                      .EmployeeAttendanceChecks
                       .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                       .OrderByDescending(check => check.Time)
                       .Select(check => check.RecognitionWay)
                       .FirstOrDefault(), requestInfo.Lang),*/
                    Status = DetermineAttendanceStatus(empAttendance.ShiftCheckInTime, empAttendance.AllowedMinutes, empAttendance.EmployeeAttendanceChecks
                       .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                       .OrderBy(check => check.FingerPrintDate)
                       .Select(check => check.FingerPrintDate)
                       .FirstOrDefault(), requestInfo.Lang),

                    LateArrivals = (empAttendance.TotalLateArrivalsHours ?? 0) + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                    EarlyDepartures = (empAttendance.TotalEarlyDeparturesHours ?? 0) + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                    WorkingHours = (empAttendance.TotalWorkingHours ?? 0) + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),

                    OverTime = (empAttendance.TotalOverTimeHours ?? 0) + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang),


                    Fingerprints = empAttendance.EmployeeAttendanceChecks
                    .Select(employeeAttendanceCheck => new GetEmployeeAttendanceInfoFingerprintDTO
                    {
                        ZoneName = employeeAttendanceCheck.Zone.Name,
                        Time = employeeAttendanceCheck.FingerPrintDate.ToString("dd-MM-yyyy hh:mm") + DateHelper.TranslateAmAndPm(employeeAttendanceCheck.FingerPrintDate.ToString("tt"), requestInfo.Lang),
                        Type = employeeAttendanceCheck.FingerPrintType == FingerPrintType.CheckIn ? TranslationHelper.GetTranslation(AmgadKeys.AttendanceRegistration, requestInfo.Lang) :
                        employeeAttendanceCheck.FingerPrintType == FingerPrintType.CheckOut ? TranslationHelper.GetTranslation(AmgadKeys.DismissalRegistration, requestInfo.Lang) :
                        employeeAttendanceCheck.FingerPrintType == FingerPrintType.BreakOut ? TranslationHelper.GetTranslation(AmgadKeys.StartABreak, requestInfo.Lang) :
                        employeeAttendanceCheck.FingerPrintType == FingerPrintType.BreakIn ? TranslationHelper.GetTranslation(AmgadKeys.FinishABreak, requestInfo.Lang) :
                        employeeAttendanceCheck.FingerPrintType == FingerPrintType.Summon ? TranslationHelper.GetTranslation(AmgadKeys.Summon, requestInfo.Lang) :
                        TranslationHelper.GetTranslation(AmgadKeys.Unknown, requestInfo.Lang),
                        RecognitionWay = employeeAttendanceCheck.RecognitionWay == RecognitionWay.FingerPrint ? TranslationHelper.GetTranslation(AmgadKeys.FingerPrint, requestInfo.Lang) :
                        employeeAttendanceCheck.RecognitionWay == RecognitionWay.NotSet ? TranslationHelper.GetTranslation(AmgadKeys.NotSet, requestInfo.Lang) :
                        employeeAttendanceCheck.RecognitionWay == RecognitionWay.FaceRecognition ? TranslationHelper.GetTranslation(AmgadKeys.FaceRecognition, requestInfo.Lang) :
                        employeeAttendanceCheck.RecognitionWay == RecognitionWay.PinRecognition ? TranslationHelper.GetTranslation(AmgadKeys.PinRecognition, requestInfo.Lang) :
                        employeeAttendanceCheck.RecognitionWay == RecognitionWay.VoiceRecognition ? TranslationHelper.GetTranslation(AmgadKeys.VoiceRecognition, requestInfo.Lang) :
                        employeeAttendanceCheck.RecognitionWay == RecognitionWay.PaternRecognition ? TranslationHelper.GetTranslation(AmgadKeys.PaternRecognition, requestInfo.Lang) :
                        employeeAttendanceCheck.RecognitionWay == RecognitionWay.PasswordRecognition ? TranslationHelper.GetTranslation(AmgadKeys.PasswordRecognition, requestInfo.Lang) :
                         TranslationHelper.GetTranslation(AmgadKeys.Unknown, requestInfo.Lang),
                    }).ToList()


                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<bool> Delete(DeleteEmployeeAttendanceModel model)
        {
            #region Validation

            var getChecks = await repositoryManager.EmployeeAttendanceCheckRepository
                .GetWithTracking(c => !c.IsDeleted && c.EmployeeAttendanceId == model.Id &&
                !c.EmployeeAttendance.IsDeleted &&
                ((model.Type == DeleteEmployeeAttendanceType.CheckIn && c.FingerPrintType == FingerPrintType.CheckIn) ||
                (model.Type == DeleteEmployeeAttendanceType.CheckOut && c.FingerPrintType == FingerPrintType.CheckOut) ||
                (model.Type == DeleteEmployeeAttendanceType.Both &&
                (c.FingerPrintType == FingerPrintType.CheckIn || c.FingerPrintType == FingerPrintType.CheckOut))))
                .ToListAsync();

            if (getChecks == null || getChecks.Count == 0)
            {
                switch (model.Type)
                {
                    case DeleteEmployeeAttendanceType.CheckIn:
                        throw new BusinessValidationException(LeillaKeys.SorryCannotFindCheckInRecord);
                    case DeleteEmployeeAttendanceType.CheckOut:
                        throw new BusinessValidationException(LeillaKeys.SorryCannotFindCheckOutRecord);
                    case DeleteEmployeeAttendanceType.Both:
                        throw new BusinessValidationException(LeillaKeys.SorryCannotFindChecksForEnteredId);
                    default:
                        break;
                }
            }
            else if (model.Type == DeleteEmployeeAttendanceType.CheckIn)
            {
                var getCheckOut = await repositoryManager.EmployeeAttendanceCheckRepository
                .GetWithTracking(c => !c.IsDeleted && c.EmployeeAttendanceId == model.Id &&
                !c.EmployeeAttendance.IsDeleted &&
                c.FingerPrintType == FingerPrintType.CheckOut)
                .AnyAsync();

                if (getCheckOut)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryCannotDeleteCheckIfBecauseThereIsCheckOutRecord);
                }
            }




            #endregion


            foreach (var item in getChecks)
            {
                item.Delete();
            }

            await unitOfWork.SaveAsync();

            return true;
        }
        public async Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations()
        {
            var currentCompanyId = requestInfo.CompanyId;
            var clientLocalDateTime = requestInfo.LocalDateTime;
            var clientLocalDate = clientLocalDateTime.Date;
            var clientLocalDateWeekDay = (WeekDay)clientLocalDateTime.DayOfWeek;
            var clientLocalTimeOnly = clientLocalDateTime.TimeOfDay;
            var newDateTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

            var employeeAttendanceRepository = repositoryManager.EmployeeAttendanceRepository;
            var requestVacationRepository = repositoryManager.RequestVacationRepository;
            var employeeRepository = repositoryManager.EmployeeRepository;

            #region Attendances

            var dayTotalAttendanceCount = await employeeAttendanceRepository.Get(c => !c.IsDeleted &&
            c.CompanyId == currentCompanyId &&
            c.EmployeeAttendanceChecks.Count() > 0 &&
            c.LocalDate.Date == clientLocalDate)
                .CountAsync();

            #endregion

            #region Vacations

            var dayTotalVacationsCount = await requestVacationRepository.Get(c => !c.Request.IsDeleted &&
            c.Request.CompanyId == currentCompanyId &&
            clientLocalDate >= c.Request.Date && clientLocalDate <= c.DateTo)
                .CountAsync() + await employeeRepository.Get(employee => !employee.IsDeleted &&
            employee.CompanyId == currentCompanyId &&
            employee.ScheduleId != null &&
            employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) == null)
                .CountAsync();

            #endregion

            #region Absences

            var dayTotalAbsencesCount = await employeeRepository.Get(employee => !employee.IsDeleted &&
            employee.CompanyId == currentCompanyId &&
            employee.ScheduleId != null &&
            employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift != null &&
            clientLocalTimeOnly > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime &&
            (employee.EmployeeAttendances == null || employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate) == null))
                .CountAsync();

            #endregion

            #region Delays

            var dayTotalDelaysCount = await employeeRepository.Get(employee => !employee.IsDeleted &&
            employee.CompanyId == currentCompanyId &&
            employee.ScheduleId != null &&
            employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift != null &&
            clientLocalTimeOnly >= employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime &&
            employee.EmployeeAttendances != null &&
            employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate) != null &&
            employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks != null &&
            employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks.FirstOrDefault(e => !e.IsDeleted && e.FingerPrintType == FingerPrintType.CheckIn) != null &&

            EF.Functions.DateDiffMinute((DateTime)(object)employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime,
            (DateTime)(object)employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks
            .FirstOrDefault(e => !e.IsDeleted && e.FingerPrintType == FingerPrintType.CheckIn).FingerPrintDate)
            > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.AllowedMinutes)
                .CountAsync();

            #endregion

            #region Handle Response

            return new GetEmployeesAttendancesInformationsResponseModel
            {
                DayTotalAttendanceCount = dayTotalAttendanceCount,
                DayTotalVacationsCount = dayTotalVacationsCount,
                DayTotalAbsencesCount = dayTotalAbsencesCount,
                DayTotalDelaysCount = dayTotalDelaysCount
            };

            #endregion

        }
        public async Task<MemoryStream> ExportDraft()
        {
            EmptyExcelDraftModelDTO employeeAttendanceHeaderDraftDTO = new();
            employeeAttendanceHeaderDraftDTO.FileName = AmgadKeys.EmployeeAttendanceEmptyDraft;
            employeeAttendanceHeaderDraftDTO.Obj = new EmployeeAttendanceHeaderDraftDTO();
            employeeAttendanceHeaderDraftDTO.ExcelExportScreen = ExcelExportScreen.EmployeeAttendance;
            return ExcelManager.ExportEmptyDraft(employeeAttendanceHeaderDraftDTO);
        }
        public async Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile)
        {
            #region Fill IniValidationModelDTO
            IniValidationModelDTO iniValidationModelDTO = new();
            iniValidationModelDTO.FileStream = importedFile;
            iniValidationModelDTO.MaxRowCount = 0;
            iniValidationModelDTO.ColumnIndexToCheckNull.AddRange(new int[] { 1, 2, 3, 4, 5, 6 });//Zone Name Lat Long can't be null
            iniValidationModelDTO.ExcelExportScreen = ExcelExportScreen.Zones;
            iniValidationModelDTO.ExpectedHeaders = typeof(EmployeeAttendanceHeaderDraftDTO).GetProperties().Select(prop => prop.Name).ToArray();
            iniValidationModelDTO.Lang = requestInfo?.Lang;
            iniValidationModelDTO.ColumnsToCheckDuplication = new List<int>();
            #endregion
            Dictionary<string, string> result = new();
            var validationMessages = ExcelValidator.InitialValidate(iniValidationModelDTO);
            if (validationMessages.Count > 0)
            {
                foreach (var kvp in validationMessages)
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }
            else
            {
                //requestInfo.CompanyId = 17;
                List<EmployeeAttendance> ImportedList = new();
                List<EmployeeAttendanceCheck> CImportedList = new();
                EmployeeAttendance Temp = new();
                EmployeeAttendanceCheck CTemp = new();
                using var workbook = new XLWorkbook(iniValidationModelDTO.FileStream);
                var worksheet = workbook.Worksheet(1);
                #region Set Employee Attendance code
                var getNextCode = await repositoryManager.EmployeeAttendanceRepository
                    .Get(e => e.CompanyId == requestInfo.CompanyId)
                    .Select(e => e.Code)
                    .DefaultIfEmpty()
                    .MaxAsync();

                #endregion
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    getNextCode++;
                    #region Check Valid Lat Long
                    double tempLatitude;
                    double tempLongtude;
                    DateTime localDate;
                    Temp = new();
                    int employeeId = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Name == row.Cell(1).GetString().Trim()).Select(es => es.Id).FirstOrDefaultAsync();
                    #endregion
                    if (employeeId != 0)
                    {
                        if (DateTime.TryParse(row.Cell(2).GetString().Trim(), out localDate))
                        {
                            if (double.TryParse(row.Cell(3).GetString().Trim(), out tempLatitude))
                            {
                                if (double.TryParse(row.Cell(4).GetString().Trim(), out tempLongtude))
                                {

                                }
                                else
                                {
                                    result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.ThisLongtudeNotValid, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                    return result;
                                }
                            }
                            else
                            {
                                result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.ThisLatitudeNotValid, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                return result;
                            }
                        }
                        else
                        {
                            result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.SorryThisLocalDateIsNotValid, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }

                    }
                    else
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(LeillaKeys.SorryEmployeeNotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }

                    #region Business Validation

                    FingerprintModel model = new()
                    {
                        Latitude = tempLatitude,
                        Longitude = tempLongtude,
                        RecognitionWay = row.Cell(6).GetString().Trim() == "FingerPrint" ? RecognitionWay.FingerPrint :
                                         row.Cell(6).GetString().Trim() == "FaceRecognition" ? RecognitionWay.FaceRecognition :
                                         row.Cell(6).GetString().Trim() == "PinRecognition" ? RecognitionWay.PinRecognition :
                                         row.Cell(6).GetString().Trim() == "PaternRecognition" ? RecognitionWay.PaternRecognition :
                                         row.Cell(6).GetString().Trim() == "VoiceRecognition" ? RecognitionWay.VoiceRecognition :
                                         RecognitionWay.NotSet,

                        Type =           row.Cell(5).GetString().Trim() == "CheckIn" ? FingerPrintType.CheckIn :
                                         row.Cell(5).GetString().Trim() == "CheckOut" ? FingerPrintType.CheckOut :
                                         row.Cell(5).GetString().Trim() == "Summon" ? FingerPrintType.Summon :
                                         row.Cell(5).GetString().Trim() == "BreakIn" ? FingerPrintType.BreakIn :
                                         row.Cell(5).GetString().Trim() == "BreakOut" ? FingerPrintType.BreakOut :
                                         FingerPrintType.NotSet,
                        FromExcel = true



                    };

                    if ((model.RecognitionWay != RecognitionWay.FingerPrint &&
                        model.RecognitionWay != RecognitionWay.FaceRecognition &&
                        model.RecognitionWay != RecognitionWay.PinRecognition &&
                        model.RecognitionWay != RecognitionWay.PaternRecognition &&
                        model.RecognitionWay != RecognitionWay.VoiceRecognition
                        ))
                    {
                        result.Add(AmgadKeys.MissMatchValue, TranslationHelper.GetTranslation(AmgadKeys.RecognitionWayValueNotCorrectPleaseLookReadMeFileToSeeExpectedValues, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                    if (model.Type != FingerPrintType.CheckIn &&
                       model.Type != FingerPrintType.CheckOut &&
                       model.Type != FingerPrintType.Summon &&
                       model.Type != FingerPrintType.BreakIn &&
                       model.Type != FingerPrintType.BreakOut)
                    {
                        result.Add(AmgadKeys.MissMatchValue, TranslationHelper.GetTranslation(AmgadKeys.FingerPrintTypeValueNotCorrectPleaseLookReadMeFileToSeeExpectedValues, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }

                    var validationResult = await employeeAttendanceBLValidation.FingerPrintValidation(model);
                    #endregion

                    var getAttandanceId = await repositoryManager
                                         .EmployeeAttendanceRepository
                                         .Get(e => !e.IsDeleted && e.EmployeeId == employeeId
                                          && e.LocalDate.Date == localDate.Date)
                                         .Select(a => a.Id)
                                         .FirstOrDefaultAsync();


                    if (getAttandanceId > 0)
                    {
                        CImportedList.Add(new EmployeeAttendanceCheck
                        {
                            EmployeeAttendanceId = getAttandanceId,
                            SummonId = validationResult.SummonId,
                            ZoneId = validationResult.ZoneId,
                            FingerPrintType = validationResult.FingerPrintType,
                            IsActive = true,
                            FingerPrintDate = localDate,
                            Latitude = model.Latitude,
                            Longitude = model.Longitude,
                            IpAddress = requestInfo.RemoteIpAddress,
                            RecognitionWay = model.RecognitionWay == RecognitionWay.NotSet ?
                            RecognitionWay.FingerPrint : model.RecognitionWay,
                            FingerprintSource = FingerprintSource.ExcelFile
                        });
                    }
                    else
                    {
                        #region Insert Employee Attendance
                        var employeeAttendance = new EmployeeAttendance
                        {
                            InsertedFromExcel = true,
                            Code = getNextCode,
                            CompanyId = requestInfo.CompanyId,
                            ScheduleId = validationResult.ScheduleId,
                            ShiftId = validationResult.ShiftId,
                            ShiftCheckInTime = validationResult.ShiftCheckInTime,
                            ShiftCheckOutTime = validationResult.ShiftCheckOutTime,
                            AllowedMinutes = validationResult.AllowedMinutes,
                            AddedApplicationType = requestInfo.ApplicationType,
                            AddUserId = requestInfo.UserId,
                            LocalDate = localDate,
                            EmployeeId = employeeId,
                            IsActive = true,
                            EmployeeAttendanceChecks = new List<EmployeeAttendanceCheck> { new EmployeeAttendanceCheck() {
                            FingerPrintType = validationResult.FingerPrintType,
                            IsActive = true,
                            ZoneId = validationResult.ZoneId,
                            FingerPrintDate = localDate,
                            FingerPrintDateUTC = DateTime.UtcNow,
                            Latitude = model.Latitude,
                            Longitude = model.Longitude,
                            IpAddress = requestInfo.RemoteIpAddress,
                            RecognitionWay = model.RecognitionWay == RecognitionWay.NotSet ?
                            RecognitionWay.FingerPrint : model.RecognitionWay }}
                        };

                        ImportedList.Add(employeeAttendance);
                        #endregion
                    }
                }
                repositoryManager.EmployeeAttendanceRepository.BulkInsert(ImportedList);
                repositoryManager.EmployeeAttendanceCheckRepository.BulkInsert(CImportedList);
                await unitOfWork.SaveAsync();
                result.Add(AmgadKeys.Success, TranslationHelper.GetTranslation(AmgadKeys.ImportedSuccessfully, requestInfo?.Lang) + LeillaKeys.Space + ImportedList.Count + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.EmployeeAttendanceEnteredSuccessfully, requestInfo?.Lang));
            }
            return result;
        }
        public async Task<List<EmployeeDailyAttendanceGroupByDayReportModel>> GetEmployeeAttendanceInPeriodReport(ReportCritria Critria)
        {

            using (var context = new ApplicationDBContext())
            {
                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@DateFrom",Critria.DateFrom),
            new SqlParameter("@DateTo", Critria.DateTo),
            new SqlParameter("@EmployeeID", Critria.EmployeeID),
            new SqlParameter("@DepartmentId", Critria.DepartmentId),
            new SqlParameter("@ZoneId", Critria.ZoneId),
            new SqlParameter("@JobTitleID", Critria.JobTitleID),
            new SqlParameter("@CompanyID", requestInfo.CompanyId)
        };
                var result = context.Database.SqlQueryRaw<EmployeeDailyAttendanceGroupByDayReportModel>("GetEmployeeAttendanceReportInAperiod @DateFrom, @DateTo, @EmployeeID, @DepartmentId, @ZoneId, @JobTitleID, @CompanyID", parameters.ToArray()).ToList();

                return result;
            }

        }
        public async Task<GetCurrentEmployeeSchedulesResponse> GetCurrentEmployeeSchedules(GetCurrentEmployeeSchedulesModel model)
        {
            var resonse = new GetCurrentEmployeeSchedulesResponse();

            #region Business Validation

            var scheduleId = await employeeAttendanceBLValidation.GetCurrentEmployeeScheduleValidation();

            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
                 throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            #endregion

            var allDatesInPeriod = OthersHelper.AllDatesInPeriod(model.DateFrom, model.DateTo).ToList();
            var currentEmployeeScheduleDayModel = new CurrentEmployeeScheduleDayModel
            {
                ScheduleId = scheduleId,
                EmployeeId = getEmployeeId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            };
            var currentEmployeeScheduleDaysAndPlans = await GetCurrentEmployeeScheduleDaysAndPlans(currentEmployeeScheduleDayModel);
            var getScheduleDays = currentEmployeeScheduleDaysAndPlans.EmployeeScheduleDays;
            var getSchedulePlans = currentEmployeeScheduleDaysAndPlans.EmployeeSchedulePlans;

            var dayModel = new CurrentEmployeeScheduleDaysAndPlansResponseModel
            {
                EmployeeScheduleDays = getScheduleDays,
                EmployeeSchedulePlans = getSchedulePlans
            };

            var schedules = new List<GetEmployeeScheduleResponseModel>();

            for (int i = 0; i < allDatesInPeriod.Count; i++)
            {
                var date = allDatesInPeriod[i];
                var currentEmployeeScheduleDaysAndPlansResponseModel = new CurrentEmployeeScheduleDaysAndPlansResponseModel
                {
                    EmployeeSchedulePlans = getSchedulePlans,
                    EmployeeScheduleDays = getScheduleDays,
                    Date = date
                };
                var day = GetCurrentEmployeeScheduleDays(currentEmployeeScheduleDaysAndPlansResponseModel);
               
                schedules.Add(new GetEmployeeScheduleResponseModel
                {
                    DayName = date.ToString("dd-MM") + LeillaKeys.Space + TranslationHelper.GetTranslation(((WeekDay)date.DayOfWeek).ToString(), requestInfo.Lang),
                    IsVacation = day.IsVacation,
                    TimeFrom = day?.StartDateTime != null ? new DateTime(day.StartDateTime.Value.Ticks).ToString("hh:mm") +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(new DateTime(day.StartDateTime.Value.Ticks).ToString("tt"), requestInfo.Lang) : null,
                    TimeTo = day?.EndDateTime != null ? new DateTime(day.EndDateTime.Value.Ticks).ToString("hh:mm") +
                    LeillaKeys.Space + TranslationHelper.GetTranslation(new DateTime(day.EndDateTime.Value.Ticks).ToString("tt"), requestInfo.Lang) : null,
                    WorkingHoursNumber = !day.IsVacation ? Math.Round((decimal)(day.EndDateTime - day.StartDateTime).Value.TotalHours, 2) : null,
                    WorkingHours = !day.IsVacation ? Math.Round((decimal)(day.EndDateTime - day.StartDateTime).Value.TotalHours, 2) + 
                    LeillaKeys.Space + 
                    TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang) : null,
                    AllowedMinutes = !day.IsVacation && day.AllowedMinutes > 0 ? day.AllowedMinutes.Value +
                    LeillaKeys.Space + 
                    TranslationHelper.GetTranslation(LeillaKeys.Minute, requestInfo.Lang) : null,
                });
            }

            resonse.TotalWorkingHours =
                Math.Round(schedules.Where(d => !d.IsVacation).Sum(d => d.WorkingHoursNumber) ?? 0, 2) + LeillaKeys.Space +
                TranslationHelper.GetTranslation(LeillaKeys.Hour, requestInfo.Lang);

            resonse.Schedules = schedules;

            return resonse;
        }
        private async Task<CurrentEmployeeScheduleDaysAndPlansResponseModel> GetCurrentEmployeeScheduleDaysAndPlans(CurrentEmployeeScheduleDayModel model)
        {
            var getEmployeeScheduleDays = await repositoryManager.ScheduleDayRepository.
                Get(s => !s.IsDeleted && s.ScheduleId == model.ScheduleId).
                Select(s => new CurrentEmployeeScheduleShiftResponseModel
                {
                    WeekDay = s.WeekDay,
                    IsVacation = s.ShiftId == null,
                    StartDateTime = s.ShiftId > 0 ? new DateTime(s.Shift.CheckInTime.Ticks) : null,
                    EndDateTime = s.ShiftId > 0 ? new DateTime(s.Shift.CheckOutTime.Ticks) : null,
                    AllowedMinutes = s.ShiftId > 0 ? s.Shift.AllowedMinutes : null
                }).ToListAsync();

            var getEmployeeSchedulePlans = await repositoryManager.EmployeeRepository.
                Get(employee => !employee.IsDeleted && employee.Id == model.EmployeeId).
                Select(employee => new CurrentEmployeeSchedulePlanResponseModel
                {
                    SchedulePlans = employee.Company.SchedulePlans.Any(sp => !sp.IsDeleted) ?
                    employee.Company.SchedulePlans.
                    Where(sp => !sp.IsDeleted &&
                    (sp.SchedulePlanEmployee != null && !sp.SchedulePlanEmployee.IsDeleted &&
                    sp.SchedulePlanEmployee.EmployeeId == employee.Id || employee.DepartmentId != null &&
                    sp.SchedulePlanDepartment != null && !sp.SchedulePlanDepartment.IsDeleted &&
                    sp.SchedulePlanDepartment.DepartmentId == employee.DepartmentId || employee.EmployeeGroups.Any(eg => !eg.IsDeleted) &&
                    sp.SchedulePlanGroup != null && !sp.SchedulePlanGroup.IsDeleted &&
                    employee.EmployeeGroups.Any(eg => !eg.IsDeleted && eg.GroupId == sp.SchedulePlanGroup.GroupId)) && sp.DateFrom.Date <= model.DateTo.Date &&
                    (sp.DateFrom.Date >= model.DateFrom.Date ||
                    sp.DateFrom.Date == employee.Company.SchedulePlans.Select(csp => csp.DateFrom.Date).Where(date => date <= model.DateFrom.Date).Max())).
                    Select(sp => new CurrentEmployeeScheduleDayResponseModel
                    {
                        DateFrom = sp.DateFrom,
                        ScheduleDays = sp.Schedule.ScheduleDays.
                        Where(sd => !sd.IsDeleted).
                        Select(s => new CurrentEmployeeScheduleShiftResponseModel
                        {
                            WeekDay = s.WeekDay,
                            IsVacation = s.ShiftId == null,
                            StartDateTime = s.ShiftId > 0 ? new DateTime(s.Shift.CheckInTime.Ticks) : null,
                            EndDateTime = s.ShiftId > 0 ? new DateTime(s.Shift.CheckOutTime.Ticks) : null,
                            AllowedMinutes = s.ShiftId > 0 ? s.Shift.AllowedMinutes : null
                        }).ToList()
                    }).ToList() : null
                }).FirstOrDefaultAsync();

            return new CurrentEmployeeScheduleDaysAndPlansResponseModel
            {
                EmployeeScheduleDays = getEmployeeScheduleDays,
                EmployeeSchedulePlans = getEmployeeSchedulePlans.SchedulePlans
            };
        }
        private CurrentEmployeeScheduleShiftResponseModel GetCurrentEmployeeScheduleDays(CurrentEmployeeScheduleDaysAndPlansResponseModel model)
        {
            var employeeScheduleDays = model.EmployeeScheduleDays;
            var employeeSchedulePlans = model.EmployeeSchedulePlans;
            List<CurrentEmployeeScheduleShiftResponseModel> days = employeeScheduleDays;

            if (employeeSchedulePlans != null && employeeSchedulePlans.Count > 0)
            {
                var planScheduleDays = employeeSchedulePlans?.
                     Where(p => p.DateFrom <= model.Date)?.
                     MaxBy(p => p.DateFrom)?.
                     ScheduleDays;

                if (planScheduleDays != null && planScheduleDays.Count > 0)
                {
                    days = planScheduleDays;
                }
            }

            var day = days.FirstOrDefault(s => s.WeekDay == (WeekDay)model.Date.DayOfWeek);

            if (day != null && day.StartDateTime != null && day.EndDateTime != null)
            {
                var startDateTime = day.StartDateTime.Value.TimeOfDay;
                var endDateTime = day.EndDateTime.Value.TimeOfDay;

                var isTwoDaysShift = TimeHelper.IsTwoDaysShift(startDateTime, endDateTime);
                
                if (isTwoDaysShift)
                {
                    day.StartDateTime = new DateTime(day.StartDateTime.Value.TimeOfDay.Ticks);
                    day.EndDateTime = new DateTime(day.EndDateTime.Value.TimeOfDay.Ticks).AddDays(1);
                }      
            }

            return day;
        }

        #region Export Report

        //public byte[] ExportEmployeeAttendanceReport(GetEmployeeAttendanceInPeriodReportParameters parameters)
        //{
        //    // Create a new FastReport instance
        //    FastReport.Report report = new FastReport.Report();

        //    // Load the report template file
        //    report.Load("your_report.frx");

        //    // Set up the data source parameters
        //    report.SetParameterValue("EmployeeID", parameters.EmployeeID);
        //    report.SetParameterValue("DateFrom", parameters.DateFrom);
        //    report.SetParameterValue("DateTo", parameters.DateTo);
        //    report.SetParameterValue("DepartmentId", parameters.DepartmentId);
        //    report.SetParameterValue("ZoneId", parameters.ZoneId);
        //    report.SetParameterValue("JobTitleID", parameters.JobTitleID);
        //    report.SetParameterValue("CompanyID", requestInfo.CompanyId);

        //    // Run the report
        //    report.Prepare();

        //    // Export the report based on the specified format
        //    switch (parameters.ExportFormat)
        //    {
        //        case ExportFormat.ViewOnly:
        //            //ShowReportInViewer(report);
        //            break;
        //        case ExportFormat.PDF:
        //           // ExportToPdf(report);
        //            break;
        //        case ExportFormat.Excel:
        //           // ExportToExcel(report);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(parameters.ExportFormat), TranslationHelper.GetTranslation(AmgadKeys.InvalidExportFormatSpecified, requestInfo.Lang));
        //    }
        //}



        #endregion

    }
}