using Dawem.Contract.BusinessLogic.Attendances;
using Dawem.Contract.BusinessValidation.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Attendances;
using Dawem.Models.Response.Dashboard;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Attendances
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
        public async Task<FingerPrintType> FingerPrint(FingerprintModel model)
        {
            #region Business Validation

            var validationResult = await employeeAttendanceBLValidation.FingerPrintValidation(model);

            #endregion

            await unitOfWork.CreateTransactionAsync();

            #region Hanlde FingerPrint

            var getAttandanceId = await repositoryManager
                .EmployeeAttendanceRepository
                .Get(e => !e.IsDeleted && e.EmployeeId == validationResult.EmployeeId
                && e.LocalDate.Date == validationResult.LocalDate.Date)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            //checkout
            if (getAttandanceId > 0)
            {
                repositoryManager.EmployeeAttendanceCheckRepository.Insert(new EmployeeAttendanceCheck
                {
                    EmployeeAttendanceId = getAttandanceId,
                    FingerPrintType = validationResult.FingerPrintType,
                    IsActive = true,
                    Time = TimeOnly.FromTimeSpan(validationResult.LocalDate.TimeOfDay),
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    IpAddress = requestInfo.RemoteIpAddress
                });
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
                    AllowedMinutes = validationResult.AllowedMinutes,
                    AddedApplicationType = requestInfo.ApplicationType,
                    AddUserId = requestInfo.UserId,
                    LocalDate = validationResult.LocalDate,
                    EmployeeId = validationResult.EmployeeId,
                    IsActive = true,
                    EmployeeAttendanceChecks = new List<EmployeeAttendanceCheck> { new EmployeeAttendanceCheck() {
                        FingerPrintType = validationResult.FingerPrintType,
                        IsActive = true,
                        Time = TimeOnly.FromTimeSpan(validationResult.LocalDate.TimeOfDay),
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        IpAddress = requestInfo.RemoteIpAddress
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
                        Time = c.Time,
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

                var checkInTime = employeeAttendance?.EmployeeAttendanceChecks?
                    .FirstOrDefault(c => c.FingerPrintType == FingerPrintType.CheckIn)?.Time;
                var checkOutTime = employeeAttendance?.EmployeeAttendanceChecks?
                    .Where(c => c.FingerPrintType == FingerPrintType.CheckOut)?
                    .OrderByDescending(c => c.Id)?.FirstOrDefault()?.Time;

                #region Check For Vacation

                var scheduleId = employeePlans.Where(s => s.DateFrom.Date <= date.Date)
                    .OrderByDescending(c => c.DateFrom.Date)?.FirstOrDefault()?.ScheduleId;

                int? shiftId = null;
                if (scheduleId != null)
                {
                    shiftId = shifts.FirstOrDefault(s => s.ScheduleId == scheduleId && s.WeekDay == (WeekDay)date.DayOfWeek)
                         .ShiftId;
                }

                #endregion

                if (scheduleId != null && shiftId == null)
                {
                    weekVacationDays.Add(new DayAndWeekDayModel()
                    {
                        Day = date.Day,
                        WeekDay = (WeekDay)date.DayOfWeek
                    });
                }
                else
                {

                    var employeeAttendanceModel = new GetEmployeeAttendancesResponseModel
                    {
                        Attendance = new GetEmployeeAttendanceModel
                        {
                            Id = employeeAttendance?.Id,
                            Day = date.Day,
                            WeekDay = (WeekDay)date.DayOfWeek,
                            WeekDayName = TranslationHelper.GetTranslation(((WeekDay)date.DayOfWeek).ToString(), requestInfo.Lang),
                            CheckInTime = checkInTime != null ?
                            checkInTime.Value.ToString("HH:mm:ss") : null,
                            CheckOutTime = checkOutTime != null ?
                            checkOutTime.Value.ToString("HH:mm:ss") : null,
                            CheckInStatus = employeeAttendance != null && checkInTime != null ? (decimal)(checkInTime.Value -
                            employeeAttendance.ShiftCheckInTime).TotalMinutes > employeeAttendance.AllowedMinutes ? EmployeeAttendanceStatus.Warning : EmployeeAttendanceStatus.Success : EmployeeAttendanceStatus.Error,
                            CheckOutStatus = checkOutTime == null ? EmployeeAttendanceStatus.Error :
                            checkOutTime < employeeAttendance.ShiftCheckOutTime ? EmployeeAttendanceStatus.Warning :
                            EmployeeAttendanceStatus.Success,
                            TotalTime = checkOutTime != null ?
                            TimeOnly.FromTimeSpan(checkOutTime.Value - checkInTime.Value).ToString("HH:mm:ss") : null
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

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response



            #endregion

            var response = await queryPaged
               .Select(empAttendance => new GetEmployeeAttendancesResponseForWebAdminModelDTO
               {
                   Id = empAttendance.Id,
                   EmployeeName = empAttendance.Employee.Name,
                   Date = empAttendance.LocalDate.Date,

                   CheckInTime =
                   empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn) != null ?
                     empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                    .Min(check => check.Time).ToString("hh:mm") + TranslateAmAndPm(empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                    .Min(check => check.Time).ToString("tt"), requestInfo.Lang) : null,

                   CheckOutTime =
                   empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut) != null ?
                     empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                    .Max(check => check.Time).ToString("hh:mm") + TranslateAmAndPm(empAttendance.EmployeeAttendanceChecks
                    .Where(check => check.FingerPrintType == FingerPrintType.CheckOut)
                    .Max(check => check.Time).ToString("tt"), requestInfo.Lang) : null,

                   WayOfRecognition = GetWayOfRecognition(
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
                       .FirstOrDefault(), requestInfo.Lang),
                   Status = DetermineAttendanceStatus(empAttendance.ShiftCheckInTime, empAttendance.AllowedMinutes, empAttendance.LocalDate, requestInfo.Lang),
                   TimeGap = CalculateTimeGap(
                     empAttendance.ShiftCheckInTime,
                     empAttendance.AllowedMinutes,
                     empAttendance.LocalDate.Date.Add(
                     empAttendance
                            .EmployeeAttendanceChecks
                             .Where(check => check.FingerPrintType == FingerPrintType.CheckIn)
                             .OrderBy(check => check.Time)
                             .Select(check => check.Time)
                             .FirstOrDefault()
                             .ToTimeSpan())),
                   ZoneName = "Zone Name"

               }).ToListAsync();

            return new GetEmployeeAttendancesResponseForWebDTO
            {
                EmployeeAttendances = response,
                TotalCount = await query.CountAsync()
            };

        }
        public static string TranslateAmAndPm(string AmOrPm, string lang)
        {
            return LeillaKeys.Space + TranslationHelper.GetTranslation(AmOrPm, lang);
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
        public static string DetermineAttendanceStatus(TimeOnly shiftCheckInTime, int allowedMinutes, DateTime localDateTime, string lang)
        {
            TimeSpan allowedTimeSpan = TimeSpan.FromMinutes(allowedMinutes);
            DateTime checkInLimit = localDateTime.Date.Add(shiftCheckInTime.ToTimeSpan());

            if (localDateTime <= checkInLimit.Add(allowedTimeSpan))
            {
                return TranslationHelper.GetTranslation(AmgadKeys.OnTime, lang); ;
            }
            else if (localDateTime > checkInLimit.Add(allowedTimeSpan))
            {
                return TranslationHelper.GetTranslation(AmgadKeys.Late, lang); ;
            }
            else
            {
                return TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang); ;
            }
        }
        public static string CalculateTimeGap(TimeOnly shiftCheckInTime, int allowedMinutes, DateTime actualCheckInTime)
        {
            DateTime scheduledCheckIn = actualCheckInTime.Date.Add(shiftCheckInTime.ToTimeSpan());
            DateTime scheduledCheckInAfterAddAllowedMinute = scheduledCheckIn.AddMinutes(allowedMinutes);
            // Calculate the time gap in minutes
            TimeSpan timeGap = actualCheckInTime - scheduledCheckInAfterAddAllowedMinute;
            // Ensure the time gap is non-negative
            TimeSpan nonNegativeTimeGap = TimeSpan.FromMinutes(Math.Max(timeGap.TotalMinutes, 0));
            // Format the non-negative time gap into HH:mm
            return nonNegativeTimeGap.ToString(@"hh\:mm");
        }
        public async Task<List<GetEmployeeAttendanceInfoDTO>> GetEmployeeAttendancesInfo(int employeeAttendanceId)
        {
            var result = await repositoryManager.EmployeeAttendanceCheckRepository.Get(s => s.EmployeeAttendanceId == employeeAttendanceId)
                .Select(r => new GetEmployeeAttendanceInfoDTO
                {
                    EmployeeName = r.EmployeeAttendance.Employee.Name,
                    LocalDate = r.EmployeeAttendance.LocalDate,
                    Time = r.Time.ToString("hh:mm") + TranslateAmAndPm(r.Time.ToString("tt"), requestInfo.Lang),
                    Type = r.FingerPrintType == FingerPrintType.CheckIn ? TranslationHelper.GetTranslation(AmgadKeys.AttendanceRegistration, requestInfo.Lang) :
                r.FingerPrintType == FingerPrintType.CheckOut ? TranslationHelper.GetTranslation(AmgadKeys.DismissalRegistration, requestInfo.Lang) :
                r.FingerPrintType == FingerPrintType.BreakOut ? TranslationHelper.GetTranslation(AmgadKeys.StartABreak, requestInfo.Lang) :
                r.FingerPrintType == FingerPrintType.BreakIn ? TranslationHelper.GetTranslation(AmgadKeys.FinishABreak, requestInfo.Lang) :
                AmgadKeys.Unknown,
                    RecognitionWay = r.RecognitionWay == RecognitionWay.FingerPrint ? TranslationHelper.GetTranslation(AmgadKeys.FingerPrint, requestInfo.Lang) :
                r.RecognitionWay == RecognitionWay.NotSet ? TranslationHelper.GetTranslation(AmgadKeys.NotSet, requestInfo.Lang) :
                r.RecognitionWay == RecognitionWay.FaceRecognition ? TranslationHelper.GetTranslation(AmgadKeys.FaceRecognition, requestInfo.Lang) :
                r.RecognitionWay == RecognitionWay.PinRecognition ? TranslationHelper.GetTranslation(AmgadKeys.PinRecognition, requestInfo.Lang) :
                r.RecognitionWay == RecognitionWay.VoiceRecognition ? TranslationHelper.GetTranslation(AmgadKeys.VoiceRecognition, requestInfo.Lang) :
                r.RecognitionWay == RecognitionWay.PaternRecognition ? TranslationHelper.GetTranslation(AmgadKeys.PaternRecognition, requestInfo.Lang) :
                r.RecognitionWay == RecognitionWay.PasswordRecognition ? TranslationHelper.GetTranslation(AmgadKeys.PasswordRecognition, requestInfo.Lang) :

                AmgadKeys.Unknown,

                }).ToListAsync();
            return result;
        }
        public async Task<bool> Delete(DeleteEmployeeAttendanceModel model)
        {
            #region Validation

            var getTimeZoneId = await repositoryManager.CompanyRepository
                .Get(c => c.Id == requestInfo.CompanyId && !c.IsDeleted)
                .Select(c => c.Country.TimeZoneId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            var clientLocalDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, getTimeZoneId).DateTime;

            var getChecks = await repositoryManager.EmployeeAttendanceCheckRepository
                .GetWithTracking(c => !c.IsDeleted && c.EmployeeAttendanceId == model.Id &&
                !c.EmployeeAttendance.IsDeleted && c.EmployeeAttendance.LocalDate.Date == clientLocalDate.Date &&
                ((model.Type == DeleteEmployeeAttendanceType.CheckIn && c.FingerPrintType == FingerPrintType.CheckIn) ||
                (model.Type == DeleteEmployeeAttendanceType.CheckOut && c.FingerPrintType == FingerPrintType.CheckOut) ||
                model.Type == DeleteEmployeeAttendanceType.Both))
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
                !c.EmployeeAttendance.IsDeleted && c.EmployeeAttendance.LocalDate.Date == clientLocalDate.Date &&
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
            var clientLocalTimeOnly = TimeOnly.FromTimeSpan(clientLocalDateTime.TimeOfDay);
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

            (EF.Functions.DateDiffMinute((DateTime)(object)employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime,
            (DateTime)(object)employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate).EmployeeAttendanceChecks
            .FirstOrDefault(e => !e.IsDeleted && e.FingerPrintType == FingerPrintType.CheckIn).Time)
            > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.AllowedMinutes))
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
    }
}