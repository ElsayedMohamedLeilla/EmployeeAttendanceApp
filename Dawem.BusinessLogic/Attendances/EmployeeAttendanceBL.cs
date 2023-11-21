using AutoMapper;
using Dawem.Contract.BusinessLogic.Schedules.Schedules;
using Dawem.Contract.BusinessValidation.Schedules.Schedules;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Response.Schedules.Schedules;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Schedules.Schedules
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
        public async Task<bool> FingerPrint()
        {
            #region Business Validation

            var result = await employeeAttendanceBLValidation.FingerPrintValidation();

            #endregion

            await unitOfWork.CreateTransactionAsync();

            #region Hanlde FingerPrint

            var getAttandance = await repositoryManager
                .EmployeeAttendanceRepository
                .GetEntityByConditionWithTrackingAsync(e => !e.IsDeleted && e.EmployeeId == result.EmployeeId
                && e.LocalDate.Date == result.LocalDate.Date);

            //checkout
            if (getAttandance != null)
            {
                getAttandance.CheckOutTime = TimeOnly.FromTimeSpan(result.LocalDate.TimeOfDay);
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
                    AddedApplicationType = requestInfo.ApplicationType,
                    AddUserId = requestInfo.UserId,
                    CheckInTime = TimeOnly.FromTimeSpan(result.LocalDate.TimeOfDay),
                    LocalDate = result.LocalDate,
                    EmployeeId = result.EmployeeId,
                    IsActive = true
                };

                repositoryManager.EmployeeAttendanceRepository.Insert(employeeAttendance);
                await unitOfWork.SaveAsync();

                #endregion
            }

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetCurrentAttendanceInfoResponseModel> GetCurrentAttendanceInfo()
        {
            #region Business Validation

            return await employeeAttendanceBLValidation.GetCurrentAttendanceInfoValidation();

            #endregion
        }
        public async Task<List<GetEmployeeAttendancesResponseModel>> GetCurrentEmployeeAttendances(GetEmployeeAttendancesCriteria model)
        {
            var resonse = new List<GetEmployeeAttendancesResponseModel>();

            #region Business Validation

            var result = await employeeAttendanceBLValidation.GetCurrentEmployeeAttendancesValidation(model);

            #endregion

            var getEmployeeId = requestInfo?.User?.EmployeeId;

            var employeeAttendances = await repositoryManager.EmployeeAttendanceRepository
                .Get(a => !a.IsDeleted && a.EmployeeId == getEmployeeId
                && a.LocalDate.Date.Month == model.Month
                && a.LocalDate.Date.Year == model.Year)
                .ToListAsync();

            var allDatesInMonth = OthersHelper.AllDatesInMonth(model.Year, model.Month).ToList();

            var weekVacationDays = new List<DayAndWeekDayModel>();

            foreach (DateTime date in allDatesInMonth)
            {
                var employeeAttendance = employeeAttendances
                        .FirstOrDefault(e => e.LocalDate.Date == date.Date);

                if (employeeAttendance == null)
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
                            Id = employeeAttendance.Id,
                            Day = date.Day,
                            WeekDay = (WeekDay)employeeAttendance.LocalDate.DayOfWeek,
                            WeekDayName = TranslationHelper.GetTranslation(((WeekDay)employeeAttendance.LocalDate.DayOfWeek).ToString(), requestInfo.Lang),
                            CheckInTime = employeeAttendance.CheckInTime,
                            CheckOutTime = employeeAttendance.CheckOutTime,
                            CheckInStatus = employeeAttendance.CheckInTime >
                            employeeAttendance.ShiftCheckInTime ? EmployeeAttendanceStatus.Warning : EmployeeAttendanceStatus.Success,
                            CheckOutStatus = employeeAttendance.CheckOutTime == null ? EmployeeAttendanceStatus.Error :
                            employeeAttendance.CheckOutTime < employeeAttendance.ShiftCheckOutTime ? EmployeeAttendanceStatus.Warning :
                            EmployeeAttendanceStatus.Success,
                            TotalTime = employeeAttendance.CheckOutTime != null ?
                            TimeOnly.FromTimeSpan(employeeAttendance.CheckOutTime.Value - employeeAttendance.CheckInTime) : null
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
                    var allVacationsText = "";

                    for (int i = 0; i < weekVacationDays.Count; i++)
                    {
                        var item = weekVacationDays[i];
                        allVacationsText += item.Day + TranslationHelper.GetTranslation(item.WeekDay.ToString(), requestInfo.Lang) +
                            (i - weekVacationDays.Count > 1 ? LeillaKeys.SpaceThenDashThenSpace : null);
                    }
                    resonse.Add(new GetEmployeeAttendancesResponseModel()
                    {
                        Attendance = null,
                        Informations = TranslationHelper.GetTranslation(LeillaKeys.EndOfWeekVacations, requestInfo.Lang) + allVacationsText
                    });

                    weekVacationDays = new List<DayAndWeekDayModel>();
                }
            }

            return null;
        }
    }
}