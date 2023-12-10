using Dawem.Contract.BusinessValidation.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Attendances;
using Dawem.Models.Response.Core.Zones;
using Dawem.Models.Response.Schedules.Schedules;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Attendances
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
            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            #region Validate Lat Long 
            var availableZonesOutput = new List<AvailableZoneDTO>();

            var employeeZones = await repositoryManager.ZoneEmployeeRepository
                               .Get(e => e.EmployeeId == getEmployeeId)
                               .Select(s => new AvailableZoneDTO
                               {
                                   Latitude = s.Zone.Latitude,
                                   Longitude = s.Zone.Longitude,
                                   Radius = s.Zone.Radius
                               })
                               .ToListAsync();
            if (employeeZones != null)
            {
                if (!IsWithinZone(model.Latitude, model.Longitude, employeeZones))
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
                                        Latitude = gz.Zone.Latitude,
                                        Longitude = gz.Zone.Longitude,
                                        Radius = gz.Zone.Radius,
                                    })
                                    .ToListAsync();
                    if (groupZones != null)
                        if (!IsWithinZone(model.Latitude, model.Longitude, groupZones))
                            throw new BusinessValidationException(AmgadKeys.SorryFingerprintingIsNotAllowedInThisArea);

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
                                          Latitude = gz.Zone.Latitude,
                                          Longitude = gz.Zone.Longitude,
                                          Radius = gz.Zone.Radius,
                                      })
                                      .ToListAsync();
                                if (departmentZones != null)
                                {
                                    if (!IsWithinZone(model.Latitude, model.Longitude, departmentZones))
                                        throw new BusinessValidationException(AmgadKeys.SorryFingerprintingIsNotAllowedInThisArea);
                                }
                            }

                        }
                }
            }

            #endregion

            var getScheduleId = await repositoryManager.EmployeeRepository
                .Get(e => e.Id == getEmployeeId && !e.IsDeleted)
                .Select(e => e.ScheduleId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeDoNotHaveSchedule);

            var getSchedule = await repositoryManager.ScheduleRepository.Get(schedule => schedule.Id == getScheduleId && !schedule.IsDeleted)
               .Select(schedule => new GetScheduleByIdResponseModel
               {
                   Id = schedule.Id,
                   Code = schedule.Code,
                   Name = schedule.Name,
                   IsActive = schedule.IsActive,
                   ScheduleDays = schedule.ScheduleDays.Select(weekShift => new ScheduleDayUpdateModel
                   {
                       Id = weekShift.Id,
                       WeekDay = weekShift.WeekDay,
                       ShiftId = weekShift.ShiftId
                   }).ToList()
               }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            var getTimeZoneId = await repositoryManager.CompanyRepository
                .Get(c => c.Id == requestInfo.CompanyId && !c.IsDeleted)
                .Select(c => c.Country.TimeZoneId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            var clientLocalDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, getTimeZoneId).DateTime;

            var shiftId = getSchedule.ScheduleDays.FirstOrDefault(d => (DayOfWeek)d.WeekDay == clientLocalDate.DayOfWeek)
                .ShiftId ?? throw new BusinessValidationException(LeillaKeys.SorryYouDoNotHaveScheduleToday);

            var shiftInfo = await repositoryManager.ShiftWorkingTimeRepository
                .Get(s => s.Id == shiftId)
                .Select(s => new
                {
                    s.CheckInTime,
                    s.CheckOutTime,
                    s.AllowedMinutes
                }).FirstOrDefaultAsync();

            return new FingerPrintValidationResponseModel
            {
                EmployeeId = getEmployeeId,
                ScheduleId = getScheduleId,
                ShiftId = shiftId,
                LocalDate = clientLocalDate,
                ShiftCheckInTime = shiftInfo.CheckInTime,
                ShiftCheckOutTime = shiftInfo.CheckOutTime,
                AllowedMinutes = shiftInfo.AllowedMinutes
            };
        }
        public async Task<GetCurrentFingerPrintInfoResponseModel> GetCurrentFingerPrintInfoValidation()
        {

            var getEmployeeId = (requestInfo?.User?.EmployeeId) ??
             throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);
            #region Get Availble Zones ForFingerprint
            var availableZonesOutput = new List<AvailableZoneDTO>();

            var employeeZones = await repositoryManager.ZoneEmployeeRepository
                               .Get(e => e.EmployeeId == getEmployeeId)
                               .Select(s => new AvailableZoneDTO
                               {
                                   Latitude = s.Zone.Latitude,
                                   Longitude = s.Zone.Longitude,
                                   Radius = s.Zone.Radius
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
                                        Latitude = gz.Zone.Latitude,
                                        Longitude = gz.Zone.Longitude,
                                        Radius = gz.Zone.Radius,
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
                                      Latitude = gz.Zone.Latitude,
                                      Longitude = gz.Zone.Longitude,
                                      Radius = gz.Zone.Radius,
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
            var getTimeZoneId = await repositoryManager.CompanyRepository
                .Get(c => c.Id == requestInfo.CompanyId && !c.IsDeleted)
                .Select(c => c.Country.TimeZoneId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScheduleNotFound);

            var clientLocalDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, getTimeZoneId).DateTime;

            var getAttendance = await repositoryManager.EmployeeAttendanceRepository
                .Get(a => !a.IsDeleted && a.EmployeeId == getEmployeeId
                && a.LocalDate.Date == clientLocalDate.Date)
                .Select(a => new GetCurrentFingerPrintInfoResponseModel
                {
                    Code = a.Code,
                    CheckInTime = a.EmployeeAttendanceChecks.FirstOrDefault(c => c.FingerPrintType == FingerPrintType.CheckIn) != null ?
                     a.EmployeeAttendanceChecks.FirstOrDefault(c => c.FingerPrintType == FingerPrintType.CheckIn).Time.ToString("HH:mm:ss") : null,
                    CheckOutTime = a.EmployeeAttendanceChecks.FirstOrDefault(c => c.FingerPrintType == FingerPrintType.CheckOut) != null ?
                     a.EmployeeAttendanceChecks.Where(c => c.FingerPrintType == FingerPrintType.CheckOut).OrderByDescending(c => c.Id).FirstOrDefault().Time.ToString("HH:mm:ss") : null,
                    LocalDate = clientLocalDate
                }).FirstOrDefaultAsync();

            return new GetCurrentFingerPrintInfoResponseModel
            {
                Code = getAttendance?.Code,
                CheckInTime = getAttendance?.CheckInTime,
                CheckOutTime = getAttendance?.CheckOutTime,
                EmployeeStatus = getAttendance?.CheckInTime == null && getAttendance?.CheckOutTime == null ? EmployeeStatus.NotAttendYet :
                getAttendance?.CheckInTime != null && getAttendance?.CheckOutTime != null ? EmployeeStatus.AttendThenLeaved :
                getAttendance?.CheckInTime != null ? EmployeeStatus.AtWork :
                EmployeeStatus.LeavedOnly,
                LocalDate = clientLocalDate,
                AvailableZones = availableZonesOutput
            };
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

        #region Get All AvailableZones
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

        public static bool IsWithinZone(double userLat, double userLon, List<AvailableZoneDTO> employeeZones)
        {
            for (int i = 0; i < employeeZones.Count; i++)
            {
                double distance = CalculateDistance(userLat, userLon, employeeZones[i].Latitude, employeeZones[i].Longitude);
                if (distance <= employeeZones[i].Radius)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
