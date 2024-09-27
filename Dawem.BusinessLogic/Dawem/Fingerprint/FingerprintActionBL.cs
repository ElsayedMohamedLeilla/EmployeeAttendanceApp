using Dawem.Contract.BusinessLogic.Dawem.Attendances;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Core;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Fingerprint;
using Dawem.Models.DTOs.Dawem.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Dawem.BusinessLogic.Dawem.Attendances
{
    public class FingerprintActionBL : IFingerprintActionBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        public FingerprintActionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
           RequestInfo _requestInfo)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestInfo;
            repositoryManager = _repositoryManager;
        }
        public async Task<bool> ReadFingerprint(ReadFingerprintModel model)
        {
            try
            {
                #region Insert Log

                var getNextCodes = await repositoryManager.FingerprintDeviceRepository
                       .Get(e => e.CompanyId == 7)
                       .Select(e => e.Code)
                       .DefaultIfEmpty()
                       .MaxAsync() + 1;

                repositoryManager.FingerprintDeviceRepository.Insert(new FingerprintDevice
                {
                    Name = "1111++" + model.Table + DateTime.UtcNow,
                    Code = getNextCodes,
                    Notes = "Data:" + "###" + model.Table,
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.Now,
                    CompanyId = 7,
                    SerialNumber = model.SN + "+" + (model.RequestBody == null ? "Null" : "NotNull")
                });
                await unitOfWork.SaveAsync();

                #endregion

                if (model.Table != "ATTLOG")
                    return true;

                var getFingerprintDevice = await repositoryManager.FingerprintDeviceRepository.
                    GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.SerialNumber == model.SN);

                if (getFingerprintDevice != null && model.RequestBody != null)
                {
                    string bodyString;
                    var getDeviceCompanyId = getFingerprintDevice.CompanyId;

                    using StreamReader reader = new(model.RequestBody, Encoding.UTF8);
                    bodyString = await reader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(bodyString) && model.Table == "ATTLOG")
                    {
                        if (!bodyString.Contains("\t"))
                        {
                            bodyString = StringHelper.FixFingerprintBody(bodyString, 12);
                        }

                        var dataRows = bodyString.Split("\n").Where(r => !string.IsNullOrEmpty(r)).ToArray();

                        if (dataRows != null && dataRows.Length > 0)
                        {
                            foreach (var dataRow in dataRows)
                            {
                                var rowDataArray = dataRow.Split("\t");
                                if (rowDataArray != null && rowDataArray.Length > 0)
                                {
                                    var fingerprintUserId = int.Parse(rowDataArray[0]);
                                    var fingerprintDateTime = DateTime.Parse(rowDataArray[1]);
                                    var fingerprintType = (FingerPrintType)int.Parse(rowDataArray[2]);

                                    var getEmployeeInfo = await repositoryManager.EmployeeRepository.
                                        Get(e => !e.IsDeleted && e.IsActive && e.CompanyId == getDeviceCompanyId &&
                                        e.FingerprintDeviceUserCode == fingerprintUserId && e.ScheduleId > 0).
                                        Select(e => new
                                        {
                                            e.Id,
                                            e.ScheduleId
                                        }).FirstOrDefaultAsync();

                                    if (getEmployeeInfo != null)
                                    {
                                        #region Insert In Fingerprint Transaction

                                        var fingerprintTransaction = new FingerprintTransaction
                                        {
                                            CompanyId = getDeviceCompanyId,
                                            FingerprintDeviceId = getFingerprintDevice.Id,
                                            EmployeeId = getEmployeeInfo.Id,
                                            ScheduleId = getEmployeeInfo.ScheduleId,
                                            FingerprintDate = fingerprintDateTime,
                                            FingerprintUserId = fingerprintUserId,
                                            FingerPrintType = fingerprintType,
                                            SerialNumber = model.SN,
                                            IsActive = true
                                        };

                                        repositoryManager.FingerprintTransactionRepository.Insert(fingerprintTransaction);
                                        getFingerprintDevice.LastSeenDateUTC = DateTime.UtcNow;
                                        await unitOfWork.SaveAsync();

                                        #endregion

                                        #region Insert Employee Attendance Check

                                        var localDateTime = DateTime.UtcNow.AddHours(requestInfo.CompanyTimeZoneToUTC);

                                        var getAttandanceId = await repositoryManager.EmployeeAttendanceRepository.
                                            Get(e => !e.IsDeleted && e.EmployeeId == getEmployeeInfo.Id && e.LocalDate.Date == localDateTime.Date).
                                            Select(a => a.Id).
                                            FirstOrDefaultAsync();

                                        //checkout
                                        if (getAttandanceId > 0)
                                        {
                                            repositoryManager.EmployeeAttendanceCheckRepository.Insert(new EmployeeAttendanceCheck
                                            {
                                                EmployeeAttendanceId = getAttandanceId,
                                                FingerPrintType = fingerprintType,
                                                IsActive = true,
                                                FingerPrintDate = localDateTime,
                                                FingerPrintDateUTC = DateTime.UtcNow,
                                                RecognitionWay = RecognitionWay.FingerPrint,
                                                FingerprintSource = FingerprintSource.FingerPrintDevice
                                            });
                                        }
                                        //checkin
                                        else
                                        {
                                            #region Insert Employee Attendance

                                            #region Set Employee Attendance code

                                            var getNextCode = await repositoryManager.EmployeeAttendanceRepository
                                                .Get(e => e.CompanyId == getDeviceCompanyId)
                                                .Select(e => e.Code)
                                                .DefaultIfEmpty()
                                                .MaxAsync() + 1;

                                            #endregion

                                            var getShift = await repositoryManager.ScheduleDayRepository.
                                                Get(sd => sd.ScheduleId == getEmployeeInfo.ScheduleId &&
                                                sd.WeekDay == (WeekDay)localDateTime.DayOfWeek).
                                                Select(e => new
                                                {
                                                    e.ShiftId,
                                                    AllowedMinutes = e.Shift != null ? e.Shift.AllowedMinutes : (int?)null,
                                                    CheckInTime = e.Shift != null ? e.Shift.CheckInTime : (TimeSpan?)null,
                                                    CheckOutTime = e.Shift != null ? e.Shift.CheckOutTime : (TimeSpan?)null,
                                                }).
                                                FirstOrDefaultAsync();

                                            var employeeAttendance = new EmployeeAttendance
                                            {
                                                Code = getNextCode,
                                                CompanyId = getDeviceCompanyId,
                                                ScheduleId = getEmployeeInfo.ScheduleId ?? 0,
                                                ShiftId = getShift?.ShiftId,
                                                ShiftCheckInTime = getShift?.CheckInTime ?? default,
                                                ShiftCheckOutTime = getShift?.CheckOutTime ?? default,
                                                AllowedMinutes = getShift?.AllowedMinutes ?? default,
                                                AddedApplicationType = ApplicationType.FingerprintDevice,
                                                LocalDate = localDateTime,
                                                EmployeeId = getEmployeeInfo.Id,
                                                IsActive = true,
                                                EmployeeAttendanceChecks = new List<EmployeeAttendanceCheck>
                                                { new() {

                                                    FingerPrintType = fingerprintType,
                                                    IsActive = true,
                                                    FingerPrintDate = localDateTime,
                                                    FingerPrintDateUTC = DateTime.UtcNow,
                                                    RecognitionWay = RecognitionWay.FingerPrint,
                                                    FingerprintSource = FingerprintSource.FingerPrintDevice
                                                } }
                                            };

                                            repositoryManager.EmployeeAttendanceRepository.Insert(employeeAttendance);

                                            #endregion
                                        }

                                        await unitOfWork.SaveAsync();

                                        #endregion
                                    }
                                }
                            }

                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #region Insert Exception

                var getNextCodes = await repositoryManager.FingerprintDeviceRepository
                       .Get(e => e.CompanyId == 7)
                       .Select(e => e.Code)
                       .DefaultIfEmpty()
                       .MaxAsync() + 1;

                repositoryManager.FingerprintDeviceRepository.Insert(new FingerprintDevice
                {
                    Name = "Exception Log" + DateTime.UtcNow,
                    Code = getNextCodes,
                    Notes = "Data:" + "###" + ex.Message,
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.Now,
                    CompanyId = 7,
                    SerialNumber = "Hatch"
                });
                await unitOfWork.SaveAsync();

                #endregion

                return false;
            }

            return false;
        }
    }
}

