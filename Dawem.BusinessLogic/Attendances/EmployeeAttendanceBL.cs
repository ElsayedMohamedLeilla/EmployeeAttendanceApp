using AutoMapper;
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
using Dawem.Models.Response.Attendances;
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
        private readonly IMapper mapper;
        public EmployeeAttendanceBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IEmployeeAttendanceBLValidation _employeeAttendanceBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            employeeAttendanceBLValidation = _employeeAttendanceBLValidation;
            mapper = _mapper;
        }
        public async Task<bool> FingerPrint(FingerprintModel model)
        {
            var response = FingerPrintType.CheckIn;

            #region Business Validation

            var result = await employeeAttendanceBLValidation.FingerPrintValidation(model);

            #endregion

            await unitOfWork.CreateTransactionAsync();

            #region Hanlde FingerPrint

            var getAttandanceId = await repositoryManager
                .EmployeeAttendanceRepository
                .Get(e => !e.IsDeleted && e.EmployeeId == result.EmployeeId
                && e.LocalDate.Date == result.LocalDate.Date)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            //checkout
            if (getAttandanceId > 0)
            {
                repositoryManager.EmployeeAttendanceCheckRepository.Insert(new EmployeeAttendanceCheck
                {
                    EmployeeAttendanceId = getAttandanceId,
                    FingerPrintType = model.Type,
                    IsActive = true,
                    Time = TimeOnly.FromTimeSpan(result.LocalDate.TimeOfDay),
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    IpAddress = requestInfo.RemoteIpAddress
                });
                response = FingerPrintType.CheckOut;
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
                    ScheduleId = result.ScheduleId,
                    ShiftId = result.ShiftId,
                    ShiftCheckInTime = result.ShiftCheckInTime,
                    ShiftCheckOutTime = result.ShiftCheckOutTime,
                    AllowedMinutes = result.AllowedMinutes,
                    AddedApplicationType = requestInfo.ApplicationType,
                    AddUserId = requestInfo.UserId,
                    LocalDate = result.LocalDate,
                    EmployeeId = result.EmployeeId,
                    IsActive = true,
                    EmployeeAttendanceChecks = new List<EmployeeAttendanceCheck> { new EmployeeAttendanceCheck() {
                        FingerPrintType = model.Type,
                        IsActive = true,
                        Time = TimeOnly.FromTimeSpan(result.LocalDate.TimeOfDay),
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
            return true;

            #endregion
        }
        public async Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfo()
        {
            #region Business Validation

            var result =  await employeeAttendanceBLValidation.GetCurrentFingerPrintInfoValidation();

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
        public async Task<List<GetEmployeeAttendancesResponseForWebAdminModelDTO>> GetEmployeeAttendancesForWebAdmin(GetEmployeeAttendancesForWebAdminCriteria criteria)
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

            var resonseTest = await queryPaged
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
                    .Min(check => check.Time).ToString("tt") , requestInfo.Lang) : null,

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

            /* var justificationsTypesList = await queryPaged.Select(e => new GetJustificationsTypeResponseModelDTO
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();

            return new GetJustificationsTypeResponseDTO
            {
                JustificationsTypes = justificationsTypesList,
                TotalCount = await query.CountAsync()
            };*/

            return resonseTest;
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
        public static double CalculateTimeGap(TimeOnly shiftCheckInTime, int allowedMinutes, DateTime actualCheckInTime)
        {
            DateTime scheduledCheckIn = actualCheckInTime.Date.Add(shiftCheckInTime.ToTimeSpan());

            DateTime scheduledCheckInAfterAddAllowedminute = scheduledCheckIn.AddMinutes(allowedMinutes);

            // Calculate the time gap in hours
            TimeSpan timeGap = actualCheckInTime - scheduledCheckInAfterAddAllowedminute;

            // Ensure the time gap is non-negative
            double hoursLate = Math.Max(timeGap.TotalHours, 0);

            return hoursLate;
        }
    }
}