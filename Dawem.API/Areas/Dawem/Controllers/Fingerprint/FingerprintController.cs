using Dawem.Contract.BusinessLogic.Dawem.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Attendances;
using Dawem.Data.UnitOfWork;
using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.DTOs.Dawem.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Schedules
{
    [ApiController]
    [Route("")]
    public class FingerprintController : DawemControllerBase
    {
        private readonly IFingerprintDeviceRepository fingerprintDeviceRepository;
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        public FingerprintController(IFingerprintDeviceRepository _fingerprintDeviceRepository,
            IUnitOfWork<ApplicationDBContext> _unitOfWork)
        {
            fingerprintDeviceRepository = _fingerprintDeviceRepository;
            unitOfWork = _unitOfWork;
        }
        [HttpPost]
        [Route("iclock/cdata")]
        public async Task<ActionResult> Read([FromQuery] PostDataModel query)
        {
            var d = 10;

            fingerprintDeviceRepository.Insert(new FingerprintDevice
            {
                Name = "New FingerPrint:",
                Notes ="Data:" + JsonConvert.SerializeObject(query),
                AddedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.Now,
                CompanyId = 7,
                SerialNumber = "Hatch"
            });
            await unitOfWork.SaveAsync();

            /*try
            {

                var device = _deviceRepository.Get(d => d.IsDeleted == false && d.Status == ResourceStatus.Active
                && d.SerialNumber == query.SN, IncludeProperties: "Branch").FirstOrDefault();
                var timezone = GetTimeZoneById(device.TimeZoneId);
                SetResponseDateHeader();

                if (device is null || device.Branch is null)
                    return Ok("OK");


                var rowsCount = 0;
                string body;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    body = await reader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(body) && query.table == "ATTLOG")
                    {
                        if (!body.Contains("\t"))
                        {
                            body = ReplaceNth(body, 12);
                        }
                        string[] rows = body.Split("\n");

                        foreach (var row in rows)
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                rowsCount++;
                                string[] rowArray = row.Split("\t");
                                if (rowArray.Length > 0)
                                {
                                    var FingerprintDateTime = DateTime.Parse(rowArray[1]);

                                    FingerprintDateTime = TimeZoneInfo.ConvertTimeToUtc(FingerprintDateTime, timezone);

                                    var diffMins = (DateTime.UtcNow - FingerprintDateTime).TotalMinutes;
                                    if (device.RecordingStratMins == 0)
                                    {
                                        device.RecordingStratMins = 1436;
                                    }
                                    if (device.RecordingStratMins < diffMins)
                                    {
                                        continue;
                                    }

                                    var user = _userRepository.Get(uf => uf.Id == int.Parse(rowArray[0]) && uf.CompanyId == device.Branch.CompanyId && !uf.IsDeleted).FirstOrDefault();
                                    if (user == null)
                                    {
                                        continue;
                                    }

                                    FingerprintTransaction transaction = new FingerprintTransaction
                                    {
                                        FingerprintDate = FingerprintDateTime,

                                        FingerprintId = int.Parse(rowArray[0]),
                                        CompanyId = device.Branch.CompanyId.Value,
                                        FingerprintTime = FingerprintDateTime.TimeOfDay,
                                        SerialNumber = query.SN,
                                        Type = 0
                                    };

                                    device.LastSeenDate = DateTime.UtcNow;
                                    device.LastFingerprintDate = FingerprintDateTime;
                                    try
                                    {
                                        _fingerprintTransactionOrch.Create(transaction);
                                        _deviceRepository.Update(device);
                                        _unitOfWork.Save();
                                    }
                                    catch (Exception ex)
                                    {
                                        await LogException(ex);
                                        return BadRequest(ex.Message);
                                    }


                                    var person = _userRepository.Get(uf => uf.Id == transaction.FingerprintId && uf.CompanyId == device.Branch.CompanyId && !uf.IsDeleted).FirstOrDefault();

                                    if (person?.ClientId != null && diffMins < Globals.ShowFingerprintScreenMins)
                                    {
                                        if (device.Branch.AllowFingerprintAttendance == false)
                                            return Ok("Ok: " + rowsCount.ToString());
                                        var gymMemberAttendViaFingerprintModel = new NewGymMemberAttendViaFingerprintModel();

                                        gymMemberAttendViaFingerprintModel.ClientId = person.ClientId.Value;
                                        //emp list
                                        var employeeIds = _employeeRepository.Get(e => e.MyUser.UserAccountSetups.Any(uas => uas.AccountSetup.FingerprintDevices.Any(fpd => fpd.SerialNumber == query.SN))
                                        && e.MyUser.UserPermissions.Any(up => up.Permission.Code == PermissionCodes.SeeClientAttendance)).Select(e => e.Id).ToList();

                                        foreach (var empId in employeeIds)
                                        {
                                            await _hubContext.Clients.Group(MagicStrings.SignalRGroupNameFirstSectionEmployee + empId + "-" + device.Branch.Id)
                                            .NewGymMemberAttendViaFingerprintOperation(gymMemberAttendViaFingerprintModel);
                                        }
                                    }

                                    else if (person?.EmployeeId != null)
                                    {
                                        //check employee Fingerprint settings 
                                        var settings = _masterSettings.Get(s => s.AccountSetupId == device.Branch.Id).FirstOrDefault();

                                        if (settings.AllowFingerprintAttendance)
                                        {
                                            var startTime = new TimeSpan();
                                            var nextDayStartTime = new TimeSpan();
                                            var endTime = new TimeSpan();
                                            bool hasSettings = false;
                                            if (settings.FPAfterCheckin != null && settings.FPBeforeCheckin != null && settings.FPBeforeCheckout != null && settings.FPAfterCheckout != null)
                                            {
                                                hasSettings = true;
                                            }

                                            var empAttendanceMethod = _employeeRepository.Get(a => a.Id == person.EmployeeId).Include(a => a.AttendMethodMaster).ThenInclude(a => a.AttendMethodMasterDetails).Select(a => a.AttendMethodMaster).FirstOrDefault();
                                            if (empAttendanceMethod == null)
                                            {
                                                var workingHours = _workingHoursRepository.Get(a => a.BranchId == device.BranchId).ToList();
                                                startTime = workingHours.FirstOrDefault(a => a.DayNo == (WeekDays)FingerprintDateTime.DayOfWeek).StartTime;
                                                endTime = workingHours.FirstOrDefault(a => a.DayNo == (WeekDays)FingerprintDateTime.DayOfWeek).FinishTime;

                                                nextDayStartTime = workingHours.FirstOrDefault(a => a.DayNo == (WeekDays)FingerprintDateTime.AddDays(1).DayOfWeek).StartTime;

                                            }
                                            else
                                            {
                                                startTime = empAttendanceMethod.AttendMethodMasterDetails.FirstOrDefault(a => a.WeekDay == (WeekDays)FingerprintDateTime.DayOfWeek).From;
                                                endTime = empAttendanceMethod.AttendMethodMasterDetails.FirstOrDefault(a => a.WeekDay == (WeekDays)FingerprintDateTime.DayOfWeek).To;
                                                nextDayStartTime = empAttendanceMethod.AttendMethodMasterDetails.FirstOrDefault(a => a.WeekDay == (WeekDays)FingerprintDateTime.AddDays(1).DayOfWeek).From;

                                            }

                                            var existedAttendance = _empAttendRepository.GetWithTracking(a => a.EmployeeId == person.EmployeeId && a.RosterDate.Value.Date == transaction.FingerprintDate.Date).FirstOrDefault();

                                            if (hasSettings)
                                            {
                                                //if has row
                                                if (existedAttendance != null)
                                                {

                                                    var beforeCheckin = startTime - new TimeSpan(3, 0, 0) - new TimeSpan(0, settings.FPBeforeCheckin.Value, 0);
                                                    var beforeCheckout = endTime - new TimeSpan(3, 0, 0) - new TimeSpan(0, settings.FPBeforeCheckout.Value, 0);
                                                    var afterCheckin = startTime - new TimeSpan(3, 0, 0) + new TimeSpan(0, settings.FPAfterCheckin.Value, 0);
                                                    var afterCheckout = endTime - new TimeSpan(3, 0, 0) + new TimeSpan(0, settings.FPAfterCheckout.Value, 0);

                                                    if (transaction.FingerprintDate.TimeOfDay >= beforeCheckin && transaction.FingerprintDate.TimeOfDay <= afterCheckin)
                                                    {
                                                        var isLate = transaction.FingerprintDate.TimeOfDay > startTime - new TimeSpan(3, 0, 0);

                                                        existedAttendance.CheckInTime = transaction.FingerprintDate.TimeOfDay;
                                                        existedAttendance.IsLate = isLate;
                                                        _empAttendRepository.Update(existedAttendance);
                                                        _unitOfWork.Save();

                                                    }
                                                    else if (transaction.FingerprintDate.TimeOfDay >= beforeCheckout && transaction.FingerprintDate.TimeOfDay <= afterCheckout)
                                                    {
                                                        existedAttendance.CheckOutTime = transaction.FingerprintDate.TimeOfDay;
                                                        _empAttendRepository.Update(existedAttendance);
                                                        _unitOfWork.Save();

                                                    }




                                                }
                                                // if has no attendnace row
                                                else
                                                {
                                                    var beforeCheckin = startTime - new TimeSpan(3, 0, 0) - new TimeSpan(0, settings.FPBeforeCheckin.Value, 0);
                                                    var beforeCheckout = endTime - new TimeSpan(3, 0, 0) - new TimeSpan(0, settings.FPBeforeCheckout.Value, 0);
                                                    var afterCheckin = startTime - new TimeSpan(3, 0, 0) + new TimeSpan(0, settings.FPAfterCheckin.Value, 0);
                                                    var afterCheckout = endTime - new TimeSpan(3, 0, 0) + new TimeSpan(0, settings.FPAfterCheckout.Value, 0);

                                                    if (transaction.FingerprintDate.TimeOfDay >= beforeCheckin && transaction.FingerprintDate.TimeOfDay <= afterCheckin)
                                                    {
                                                        var isLate = transaction.FingerprintDate.TimeOfDay > startTime - new TimeSpan(3, 0, 0);
                                                        var newAttend = new EmpAttend()
                                                        {
                                                            FingerprintCode = transaction.FingerprintId,
                                                            AttendanceMethod = AttendanceMethod.Fingerprint,
                                                            CheckInTime = transaction.FingerprintDate.TimeOfDay,
                                                            RosterDate = transaction.FingerprintDate,
                                                            WeekDay = (int)transaction.FingerprintDate.DayOfWeek,
                                                            EmployeeId = person.EmployeeId.Value,
                                                            AccountSetupId = device.Branch.Id,
                                                            EmployeeStatus = EmployeeAttendStatus.Attendee,
                                                            IsLate = isLate,
                                                            AddedDate = DateTime.Now
                                                        };
                                                        _empAttendRepository.Insert(newAttend);
                                                        _unitOfWork.Save();
                                                    }
                                                    else if (transaction.FingerprintDate.TimeOfDay >= beforeCheckout && transaction.FingerprintDate.TimeOfDay <= afterCheckout)
                                                    {
                                                        var newAttend = new EmpAttend()
                                                        {
                                                            FingerprintCode = transaction.FingerprintId,
                                                            AttendanceMethod = AttendanceMethod.Fingerprint,
                                                            CheckOutTime = transaction.FingerprintDate.TimeOfDay,
                                                            RosterDate = transaction.FingerprintDate,
                                                            WeekDay = (int)transaction.FingerprintDate.DayOfWeek,
                                                            EmployeeId = person.EmployeeId.Value,
                                                            AccountSetupId = device.Branch.Id,
                                                            EmployeeStatus = EmployeeAttendStatus.Attendee,
                                                            AddedDate = DateTime.Now
                                                        };
                                                        _empAttendRepository.Insert(newAttend);
                                                        _unitOfWork.Save();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (existedAttendance == null)
                                                {
                                                    var isLate = transaction.FingerprintDate.TimeOfDay > startTime - new TimeSpan(3, 0, 0);
                                                    var newAttend = new EmpAttend()
                                                    {
                                                        FingerprintCode = transaction.FingerprintId,
                                                        AttendanceMethod = AttendanceMethod.Fingerprint,
                                                        CheckInTime = transaction.FingerprintDate.TimeOfDay,
                                                        RosterDate = transaction.FingerprintDate,
                                                        WeekDay = (int)transaction.FingerprintDate.DayOfWeek,
                                                        EmployeeId = person.EmployeeId.Value,
                                                        AccountSetupId = device.Branch.Id,
                                                        EmployeeStatus = EmployeeAttendStatus.Attendee,
                                                        IsLate = isLate,
                                                        AddedDate = DateTime.Now
                                                    };
                                                    _empAttendRepository.Insert(newAttend);
                                                    _unitOfWork.Save();
                                                }
                                                else
                                                {
                                                    if (existedAttendance.CheckOutTime == null)
                                                    {
                                                        existedAttendance.CheckOutTime = transaction.FingerprintDate.TimeOfDay;
                                                        _empAttendRepository.Update(existedAttendance);
                                                    }
                                                }

                                            }

                                        }
                                    }
                                }
                            }
                        }


                    }
                    else if (!string.IsNullOrEmpty(body) && query.table == "OPERLOG")
                    {
                        var rows = body.Split("\n").Where(r => !string.IsNullOrEmpty(r)).ToList();
                        await LogData(body, query.SN);
                        rowsCount += rows.Count();
                        var userRows = rows.Where(r => r.Contains("USER") && !r.Contains("USERPIC"));

                        if (userRows.Any())
                        {
                            var userRowsDetails = userRows.Select(ur => ur.Split("\t"));

                            userRowsDetails = userRowsDetails.Select(ur => ur.Select(r => r.Substring(r.IndexOf('=') + 1)).ToArray());
                            var fingerprintUsers = userRowsDetails.Select(ur => new FingerprintUser
                            {
                                Id = Convert.ToInt32(ur[0]),
                                Name = ur[1],
                                Pri = ur[2],
                                Password = ur[3],
                                Card = ur[4],
                                Grp = ur[5],
                                TZ = ur[6],
                                Verify = ur[7],
                                AccountSetupId = device.BranchId,
                                CompanyId = device.Branch.CompanyId.Value,
                                SerialNumber = query.SN,
                                Status = ResourceStatus.Active,
                                IsDeleted = false

                            });
                            await _userRepository.BulkInsertOrUpdate(fingerprintUsers,
                               new List<string> { nameof(FingerprintUser.ClientId), nameof(FingerprintUser.EmployeeId) });
                            await _unitOfWork.SaveAsync();
                        }

                        var fbRows = rows.Where(r => r.Contains("FP"));

                        if (fbRows.Any())
                        {
                            var fbRowsDetails = fbRows.Select(ur => ur.Split("\t"));

                            fbRowsDetails = fbRowsDetails.Select(ur => ur.Select(r => r.Substring(r.IndexOf('=') + 1)).ToArray());
                            var fingerprintTemps = fbRowsDetails.Select(ur => new FingerprintTMP
                            {
                                Id = Convert.ToInt32(ur[1]),
                                FPUserId = Convert.ToInt32(ur[0]),
                                Size = ur[2],
                                TMP = ur[4],
                                BranchId = device.BranchId.Value,
                                CompanyId = device.Branch.CompanyId.Value,
                            });
                            await _fingerprintTMPRepository.BulkInsertOrUpdate(fingerprintTemps,
                                new List<string>());
                            await _unitOfWork.SaveAsync();
                        }

                    }
                    return Ok("Ok: " + rowsCount.ToString());
                }
            }
            catch (Exception ex)
            {
                await LogException(ex);
                return Ok("Ok");
            }*/
            return Ok("Ok");
        }
    }
}