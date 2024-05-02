using Dawem.Contract.BusinessValidation.Dawem.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Schedules.Schedules;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Attendances;
using Dawem.Models.Response.Dawem.Core.Zones;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Attendances
{

    public class EmployeeAttendanceBLValidation : IEmployeeAttendanceBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public EmployeeAttendanceBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<FingerPrintValidationResponseModel> FingerPrintValidation(FingerprintModel model)
        {
            #region First Handle

            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
                    throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var getEmployee = await repositoryManager.EmployeeRepository
                .GetByIdAsync(getEmployeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            var getScheduleId = getEmployee.ScheduleId ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeDoNotHaveSchedule);

            var clientLocalDateTime = requestInfo.LocalDateTime;
            var clientLocalDate = requestInfo.LocalDateTime.Date; 

            #endregion

            #region Validate Latitude And Longitude 

            int? zoneId = null;

            var availableZonesOutput = new List<AvailableZoneDTO>();

            var employeeZones = await repositoryManager.ZoneEmployeeRepository
                               .Get(e => e.EmployeeId == getEmployeeId)
                               .Select(s => new AvailableZoneDTO
                               {
                                   ZoneId = s.Zone.Id,
                                   Name = s.Zone.Name,
                                   Latitude = s.Zone.Latitude,
                                   Longitude = s.Zone.Longitude,
                                   Radius = s.Zone.Radius
                               })
                               .ToListAsync();
            if (employeeZones != null)
            {
                zoneId = IsWithinZone(model.Latitude, model.Longitude, employeeZones);
                if (zoneId == null || zoneId == 0)
                    throw new BusinessValidationException(AmgadKeys.SorryFingerprintingIsNotAllowedInThisArea);
            }
            else
            {

                var employeeGroups = await repositoryManager.GroupEmployeeRepository
                                    .Get(g => g.EmployeeId == getEmployeeId)
                                    .Select(g => g.GroupId)
                                    .ToListAsync();
                if (employeeGroups != null)
                {
                    var groupZones = await repositoryManager.ZoneGroupRepository
                                    .Get(gz => employeeGroups.Contains(gz.GroupId))
                                    .Select(gz => new AvailableZoneDTO
                                    {
                                        ZoneId = gz.Zone.Id,
                                        Name = gz.Zone.Name,
                                        Latitude = gz.Zone.Latitude,
                                        Longitude = gz.Zone.Longitude,
                                        Radius = gz.Zone.Radius,
                                    })
                                    .ToListAsync();
                    if (groupZones != null)
                    {
                        zoneId = IsWithinZone(model.Latitude, model.Longitude, groupZones);
                        if (zoneId == null || zoneId == 0)
                            throw new BusinessValidationException(AmgadKeys.SorryFingerprintingIsNotAllowedInThisArea);
                    }

                    else //check employee department zones
                    {
                        var employeeDepartmentId = await repositoryManager.EmployeeRepository
                                    .Get(g => g.Id == getEmployeeId)
                                    .Select(g => g.DepartmentId).FirstOrDefaultAsync();
                        if (employeeDepartmentId != null)
                        {
                            var departmentZones = await repositoryManager.ZoneDepartmentRepository
                                  .Get(gz => gz.DepartmentId == employeeDepartmentId)
                                  .Select(gz => new AvailableZoneDTO
                                  {
                                      ZoneId = gz.Zone.Id,
                                      Name = gz.Zone.Name,
                                      Latitude = gz.Zone.Latitude,
                                      Longitude = gz.Zone.Longitude,
                                      Radius = gz.Zone.Radius,
                                  })
                                  .ToListAsync();
                            if (departmentZones != null)
                            {
                                zoneId = IsWithinZone(model.Latitude, model.Longitude, departmentZones);
                                if (zoneId == null || zoneId == 0)
                                    throw new BusinessValidationException(AmgadKeys.SorryFingerprintingIsNotAllowedInThisArea);
                            }
                        }

                    }
                }
            }

            #endregion

            #region Check If Fingerprint Cross Days ( 24 )

            var checkIfShift24HoursModel = new CheckIfShift24HoursModel
            {
                ScheduleId = getScheduleId,
                ClientLocalDateTime = clientLocalDateTime,
                EmployeeId = getEmployeeId
            };

            var response = await CheckIfShiftIs24Hours(checkIfShift24HoursModel);

            clientLocalDateTime = response.ClientLocalDateTime;
            clientLocalDate = response.ClientLocalDateTime.Date;
            var getSchedule = response.Schedule;
            var shiftInfo = response.ShiftInfo;

            #endregion

            #region Validate Checks

            var fingerPrintTypes = await repositoryManager
                .EmployeeAttendanceCheckRepository
                .Get(e => !e.IsDeleted && e.EmployeeAttendance.EmployeeId == getEmployeeId
                && e.EmployeeAttendance.LocalDate.Date == clientLocalDate)
                .Select(a => a.FingerPrintType)
                .ToListAsync();

            var fingerPrintType = FingerPrintType.CheckIn;

            if (fingerPrintTypes == null || fingerPrintTypes.Count <= 0)
            {
                if (model.Type == FingerPrintType.Summon)
                    throw new BusinessValidationException(LeillaKeys.SorryCannotDoSummonFingerprintOutsideWorkingHours);
                else
                    fingerPrintType = FingerPrintType.CheckIn;

            }
            else if (fingerPrintTypes.Contains(FingerPrintType.CheckIn) && fingerPrintTypes.Contains(FingerPrintType.CheckOut))
            {
                if (model.Type == FingerPrintType.Summon)
                    throw new BusinessValidationException(LeillaKeys.SorryCannotDoSummonFingerprintOutsideWorkingHours);
                else
                    throw new BusinessValidationException(LeillaKeys.SorryYouAlreadyDoneRegisterCheckInAndCheckOutInCurrentDay);

            }
            else if (fingerPrintTypes.Contains(FingerPrintType.CheckIn))
            {
                if (model.Type == FingerPrintType.Summon)
                    fingerPrintType = FingerPrintType.Summon;
                else
                    fingerPrintType = FingerPrintType.CheckOut;
            }

            #endregion

            #region Validate Summon

            int? summonId = null;

            if (model.Type == FingerPrintType.Summon)
            {
                summonId = await repositoryManager.SummonRepository
                    .Get(s => !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && clientLocalDateTime >= s.LocalDateAndTime &&
                    (s.TimeType == TimeType.Second && EF.Functions.DateDiffSecond(s.LocalDateAndTime, clientLocalDateTime) <= s.AllowedTime ||
                    s.TimeType == TimeType.Minute && EF.Functions.DateDiffMinute(s.LocalDateAndTime, clientLocalDateTime) <= s.AllowedTime ||
                    s.TimeType == TimeType.Hour && EF.Functions.DateDiffHour(s.LocalDateAndTime, clientLocalDateTime) <= s.AllowedTime) &&
                    (s.ForAllEmployees.HasValue && s.ForAllEmployees.Value ||
                    s.SummonEmployees != null && s.SummonEmployees.Any(e => !e.IsDeleted && e.EmployeeId == getEmployeeId) ||
                    s.SummonGroups != null && s.SummonGroups.Any(sg => !sg.IsDeleted && sg.Group.GroupEmployees != null && sg.Group.GroupEmployees.Any(ge => !ge.IsDeleted && ge.EmployeeId == getEmployeeId)) ||
                    s.SummonDepartments != null && s.SummonDepartments.Any(sd => !sd.IsDeleted && sd.Department.Employees != null && sd.Department.Employees.Any(e => !e.IsDeleted && e.Id == getEmployeeId))))
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();

                if (summonId <= 0)
                    throw new BusinessValidationException(LeillaKeys.SorryNotAllowedToDoSummonFingerprintAtCurrentTimeThereIsNoSummon);

                var checkDoneBefore = await repositoryManager.EmployeeAttendanceCheckRepository.
                    Get(c => !c.IsDeleted && c.SummonId == summonId && c.FingerPrintType == FingerPrintType.Summon).
                    AnyAsync();

                if (checkDoneBefore)
                    throw new BusinessValidationException(LeillaKeys.SorryYouAlreadyDoneThisSummonBefore);
            }

            #endregion

            #region Validate Fingerprint Device Code

            if (!string.IsNullOrEmpty(getEmployee.FingerprintMobileCode) &&
                !string.IsNullOrWhiteSpace(getEmployee.FingerprintMobileCode) &&
                !model.FromExcel)
            {
                if ((string.IsNullOrEmpty(model.FingerprintMobileCode) ||
                    string.IsNullOrWhiteSpace(model.FingerprintMobileCode)) && getEmployee.Id != 13)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterEmployeeFingerprintMobileCode);
                }
                else if (model.FingerprintMobileCode != getEmployee.FingerprintMobileCode && getEmployee.Id != 13)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryFingerprintAllowedOnlyFromCurrentEmployeePersonalMobile);
                }
            }

            #endregion

            return new FingerPrintValidationResponseModel
            {
                EmployeeId = getEmployeeId,
                ScheduleId = getScheduleId,
                ShiftId = shiftInfo.ShiftId,
                SummonId = summonId,
                ZoneId = zoneId,
                LocalDateTime = clientLocalDateTime,
                ShiftCheckInTime = shiftInfo.ShiftCheckInTime,
                ShiftCheckOutTime = shiftInfo.ShiftCheckOutTime,
                AllowedMinutes = shiftInfo.ShiftAllowedMinutes,
                FingerPrintType = fingerPrintType
            };
        }
        public async Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfoValidation()
        {
            #region First Handle

            var getEmployeeId = requestInfo.EmployeeId ??
                 throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var getEmployee = await repositoryManager.EmployeeRepository
                .GetByIdAsync(getEmployeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            var getScheduleId = getEmployee.ScheduleId ??
               throw new BusinessValidationException(LeillaKeys.SorryEmployeeDoNotHaveSchedule);

            var clientLocalDateTime = requestInfo.LocalDateTime;
            var clientLocalDate = requestInfo.LocalDateTime.Date; 

            #endregion

            #region Get Availble Zones For Fingerprint

            var availableZonesOutput = new List<AvailableZoneDTO>();

            var employeeZones = await repositoryManager.ZoneEmployeeRepository
                               .Get(e => e.EmployeeId == getEmployeeId)
                               .Select(s => new AvailableZoneDTO
                               {
                                   Name = s.Zone.Name,
                                   Latitude = s.Zone.Latitude,
                                   Longitude = s.Zone.Longitude,
                                   Radius = s.Zone.Radius ?? 0
                               })
                               .ToListAsync();
            if (employeeZones != null)
            {
                availableZonesOutput = employeeZones;
            }
            else
            {

                var employeeGroups = await repositoryManager.GroupEmployeeRepository
                                    .Get(g => g.EmployeeId == getEmployeeId)
                                    .Select(g => g.GroupId)
                                    .ToListAsync();
                if (employeeGroups != null)
                {
                    var groupZones = await repositoryManager.ZoneGroupRepository
                                    .Get(gz => employeeGroups.Contains(gz.GroupId))
                                    .Select(gz => new AvailableZoneDTO
                                    {
                                        Name = gz.Zone.Name,
                                        Latitude = gz.Zone.Latitude,
                                        Longitude = gz.Zone.Longitude,
                                        Radius = gz.Zone.Radius ?? 0,
                                    })
                                    .ToListAsync();
                    if (groupZones != null)
                        availableZonesOutput = groupZones;

                    else //check employee department zones
                    {
                        var employeeDepartmentId = await repositoryManager.EmployeeRepository
                                    .Get(g => g.Id == getEmployeeId)
                                    .Select(g => g.DepartmentId).FirstOrDefaultAsync();
                        if (employeeDepartmentId != null)
                        {
                            var departmentZones = await repositoryManager.ZoneDepartmentRepository
                                  .Get(gz => gz.DepartmentId == employeeDepartmentId)
                                  .Select(gz => new AvailableZoneDTO
                                  {
                                      Name = gz.Zone.Name,
                                      Latitude = gz.Zone.Latitude,
                                      Longitude = gz.Zone.Longitude,
                                      Radius = gz.Zone.Radius ?? 0,
                                  })
                                  .ToListAsync();
                            if (departmentZones != null)
                            {
                                availableZonesOutput = departmentZones;
                            }
                        }

                    }
                }
            }

            #endregion

            #region Check If Fingerprint Cross Days ( 24 )

            var checkIfShift24HoursModel = new CheckIfShift24HoursModel
            {
                ScheduleId = getScheduleId,
                ClientLocalDateTime = clientLocalDateTime,
                EmployeeId = getEmployeeId
            };

            var response = await CheckIfShiftIs24Hours(checkIfShift24HoursModel);

            clientLocalDateTime = response.ClientLocalDateTime;
            clientLocalDate = response.ClientLocalDateTime.Date;
            var getSchedule = response.Schedule;
            var shiftInfo = response.ShiftInfo;

            #endregion

            #region Get Attendance

            var getAttendance = await repositoryManager.EmployeeAttendanceRepository
                .Get(a => !a.IsDeleted && a.EmployeeId == getEmployeeId
                && a.LocalDate.Date == clientLocalDateTime.Date)
                .Select(a => new GetCurrentFingerPrintInfoResponseModel
                {
                    Id = a.Id,
                    Code = a.Code,
                    CheckInDateTime = a.EmployeeAttendanceChecks.FirstOrDefault(c => !c.IsDeleted && c.FingerPrintType == FingerPrintType.CheckIn) != null ?
                     a.EmployeeAttendanceChecks.FirstOrDefault(c => !c.IsDeleted && c.FingerPrintType == FingerPrintType.CheckIn).FingerPrintDate : default,
                    CheckOutDateTime = a.EmployeeAttendanceChecks.FirstOrDefault(c => !c.IsDeleted && c.FingerPrintType == FingerPrintType.CheckOut) != null ?
                     a.EmployeeAttendanceChecks.Where(c => !c.IsDeleted && c.FingerPrintType == FingerPrintType.CheckOut).OrderByDescending(c => c.Id).FirstOrDefault().FingerPrintDate : default,
                    LocalDate = clientLocalDateTime
                }).FirstOrDefaultAsync();

            #endregion

            #region Check If Summon

            var checkIfHasSummon = await repositoryManager.SummonRepository
                   .Get(s => !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && clientLocalDateTime >= s.LocalDateAndTime &&
                   (s.TimeType == TimeType.Second && EF.Functions.DateDiffSecond(s.LocalDateAndTime, clientLocalDateTime) <= s.AllowedTime ||
                   s.TimeType == TimeType.Minute && EF.Functions.DateDiffMinute(s.LocalDateAndTime, clientLocalDateTime) <= s.AllowedTime ||
                   s.TimeType == TimeType.Hour && EF.Functions.DateDiffHour(s.LocalDateAndTime, clientLocalDateTime) <= s.AllowedTime) &&
                   !s.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.EmployeeAttendance.EmployeeId == getEmployeeId && eac.SummonId == s.Id) &&
                   ((s.ForAllEmployees.HasValue && s.ForAllEmployees.Value) ||
                   (s.SummonEmployees != null && s.SummonEmployees.Any(e => !e.IsDeleted && e.EmployeeId == getEmployeeId)) ||
                   (s.SummonGroups != null && s.SummonGroups.Any(sg => !sg.IsDeleted && sg.Group.GroupEmployees != null && sg.Group.GroupEmployees.Any(ge => !ge.IsDeleted && ge.EmployeeId == getEmployeeId))) ||
                   (s.SummonDepartments != null && s.SummonDepartments.Any(sd => !sd.IsDeleted && sd.Department.Employees != null && sd.Department.Employees.Any(e => !e.IsDeleted && e.Id == getEmployeeId)))))
                   .AnyAsync();

            #endregion

            return new GetCurrentFingerPrintInfoResponseModel
            {
                Id = getAttendance?.Id,
                Code = getAttendance?.Code,
                CheckInDateTime = getAttendance.CheckInDateTime,
                CheckOutDateTime = getAttendance.CheckOutDateTime,

                DefaultCheckType = getAttendance?.CheckInDateTime == null &&
                getAttendance?.CheckOutDateTime == null ? FingerprintCheckType.CheckIn :
                getAttendance?.CheckInDateTime != null && getAttendance?.CheckOutDateTime != null ? FingerprintCheckType.NotDefined :
                getAttendance?.CheckInDateTime != null ? checkIfHasSummon ? FingerprintCheckType.Summon : FingerprintCheckType.CheckOut :
                FingerprintCheckType.NotDefined,

                EmployeeStatus = getAttendance?.CheckInDateTime == null && getAttendance?.CheckOutDateTime == null ? EmployeeStatus.NotAttendYet :
                getAttendance?.CheckInDateTime != null && getAttendance?.CheckOutDateTime != null ? EmployeeStatus.AttendThenLeaved :
                getAttendance?.CheckInDateTime != null ? EmployeeStatus.AtWork :
                EmployeeStatus.LeavedOnly,

                LocalDate = clientLocalDateTime,
                AvailableZones = availableZonesOutput
            };
        }
        private async Task<CheckIfShiftIs24HoursResponseModel> CheckIfShiftIs24Hours(CheckIfShift24HoursModel model)
        {
            #region Get Schedule

            var getSchedule = await repositoryManager.ScheduleRepository.Get(schedule => schedule.Id == model.ScheduleId &&
                !schedule.IsDeleted)
                   .Select(schedule => new GetScheduleModel
                   {
                       Id = schedule.Id,
                       Code = schedule.Code,
                       Name = schedule.Name,
                       IsActive = schedule.IsActive,
                       ScheduleDays = schedule.ScheduleDays.Select(weekShift => new GetScheduleDayModel
                       {
                           Id = weekShift.Id,
                           WeekDay = weekShift.WeekDay,
                           ShiftId = weekShift.ShiftId,
                           ShiftInfo = weekShift.Shift != null ? new ShiftInfoModel
                           {
                               ShiftId = weekShift.Shift.Id,
                               ShiftCheckInTime = weekShift.Shift.CheckInTime,
                               ShiftCheckOutTime = weekShift.Shift.CheckOutTime,
                               ShiftAllowedMinutes = weekShift.Shift.AllowedMinutes
                           } : null
                       }).ToList()
                   }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            #endregion

            var clientLocalDateTime = model.ClientLocalDateTime;
            var clientLocalDate = model.ClientLocalDateTime.Date;

            #region Check If Fingerprint Cross Days ( 24 )

            ShiftInfoModel the24HoursShift = null;

            var getLastFingerprint = await repositoryManager.
                EmployeeAttendanceCheckRepository.
                Get(e => !e.IsDeleted && e.EmployeeAttendance.EmployeeId == model.EmployeeId).
                Select(check => new
                {
                    check.FingerPrintDate,
                    check.FingerPrintType,
                    check.EmployeeAttendance.LocalDate
                }).LastOrDefaultAsync();

            var checkIfLastFingerPrintAllow24Hours =
                getLastFingerprint != null &&
                getLastFingerprint.FingerPrintType == FingerPrintType.CheckIn &&
                (clientLocalDate - getLastFingerprint.LocalDate).Days == 1 &&
                (clientLocalDate - getLastFingerprint.FingerPrintDate.Date).Days == 1;

            if (checkIfLastFingerPrintAllow24Hours)
            {
                var lastClientLocalDateTime = requestInfo.LocalDateTime.AddDays(-1);
                var lastClientLocalDate = requestInfo.LocalDateTime.Date.AddDays(-1);

                var getLastShiftInfo = getSchedule.ScheduleDays.
                    FirstOrDefault(d => (DayOfWeek)d.WeekDay == lastClientLocalDateTime.DayOfWeek).
                    ShiftInfo;

                if (getLastShiftInfo != null)
                {
                    #region Check If Shift Is 24 Hours Cross Days

                    var shiftCheckInTime = getLastShiftInfo.ShiftCheckInTime;
                    var shiftCheckOutTime = getLastShiftInfo.ShiftCheckOutTime;

                    var shiftCheckInTimeType = shiftCheckInTime.Hours >= 12 ? AmPm.PM : AmPm.AM;
                    var shiftCheckOutTimeType = shiftCheckOutTime.Hours >= 12 ? AmPm.PM : AmPm.AM;

                    var is24HoursShift = ((shiftCheckInTimeType == AmPm.PM && shiftCheckOutTimeType == AmPm.AM) ||
                        (shiftCheckInTimeType == AmPm.AM && shiftCheckOutTimeType == AmPm.AM && shiftCheckInTime > shiftCheckOutTime) ||
                        (shiftCheckInTimeType == AmPm.PM && shiftCheckOutTimeType == AmPm.PM && shiftCheckInTime > shiftCheckOutTime))
                        && clientLocalDateTime.TimeOfDay <= shiftCheckOutTime;

                    if (is24HoursShift)
                    {
                        the24HoursShift = getLastShiftInfo;
                        clientLocalDate = lastClientLocalDate;
                        lastClientLocalDate = lastClientLocalDateTime;
                    }

                    #endregion
                }
            }


            var shiftInfo = the24HoursShift != null ? the24HoursShift :
               (getSchedule.ScheduleDays.FirstOrDefault(d => (DayOfWeek)d.WeekDay == clientLocalDateTime.DayOfWeek)
               .ShiftInfo ?? throw new BusinessValidationException(LeillaKeys.SorryYouDoNotHaveScheduleToday));


            #endregion

            var respnse = new CheckIfShiftIs24HoursResponseModel()
            {
                ClientLocalDateTime = clientLocalDateTime,
                ShiftInfo = shiftInfo,
                Schedule = getSchedule
            };

            return respnse;
        }
        public async Task<bool> GetEmployeeAttendancesValidation(GetEmployeeAttendancesCriteria model)
        {
            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
                 throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var checkIfHasAttendances = await repositoryManager.EmployeeAttendanceRepository
                .Get(a => !a.IsDeleted && a.EmployeeId == getEmployeeId
                && a.LocalDate.Date.Month == model.Month
                && a.LocalDate.Date.Year == model.Year)
                .AnyAsync();

            if (!checkIfHasAttendances)
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoAttendancesInSelectedYearAndMonth);

            return true;
        }
        public List<AvailableZoneWithoutRadiusDTO> GetAvailableLatLongForEmployee(List<AvailableZoneDTO> employeeZones)
        {
            var availableLatLongList = new List<AvailableZoneWithoutRadiusDTO>();
            double stepSize = 0.01; // Adjust the step size for performance 

            foreach (var employeeZone in employeeZones)
            {
                double employeeLatitude = employeeZone.Latitude;
                double employeeLongitude = employeeZone.Longitude;
                double? radius = employeeZone.Radius;

                if (radius.HasValue)
                {
                    // Calculate borders around the zone
                    double northBorder = employeeLatitude + (double)radius / 111; // 1 degree latitude is approximately 111 km
                    double southBorder = employeeLatitude - (double)radius / 111;
                    double eastBorder = employeeLongitude + (double)radius / (111 * Math.Cos(Math.Abs(employeeLatitude) * (Math.PI / 180)));
                    double westBorder = employeeLongitude - (double)radius / (111 * Math.Cos(Math.Abs(employeeLatitude) * (Math.PI / 180)));

                    // Iterate through the grid within the borders
                    for (double lat = southBorder; lat <= northBorder; lat += stepSize)
                    {
                        for (double lon = westBorder; lon <= eastBorder; lon += stepSize)
                        {
                            var distance = CalculateDistance(employeeLatitude, employeeLongitude, lat, lon);

                            if (distance <= (double)radius)
                            {
                                availableLatLongList.Add(new AvailableZoneWithoutRadiusDTO { Latitude = lat, Longitude = lon });
                            }
                        }
                    }
                }
            }

            return availableLatLongList;
        }
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadius = 6371; // Earth's radius in kilometers
            // Convert latitude and longitude from degrees to radians
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EarthRadius * c;

            return distance; // Distance between the two coordinates in kilometers
        }
        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static int? IsWithinZone(double userLat, double userLon, List<AvailableZoneDTO> employeeZones)
        {
            for (int i = 0; i < employeeZones.Count; i++)
            {
                double distance = CalculateDistance(userLat, userLon, employeeZones[i].Latitude, employeeZones[i].Longitude);
                if (distance <= employeeZones[i].Radius)
                {
                    return employeeZones[i].ZoneId;
                }
            }
            return null;
        }
    }
}
