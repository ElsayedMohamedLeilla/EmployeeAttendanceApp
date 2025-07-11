﻿using Dawem.Data;
using Dawem.Domain.Entities.Attendances;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Dashboard;

namespace Dawem.Contract.Repository.Attendances
{
    public interface IEmployeeAttendanceRepository : IGenericRepository<EmployeeAttendance>
    {
        IQueryable<EmployeeAttendance> GetAsQueryable(GetEmployeeAttendancesForWebAdminCriteria criteria);
        IQueryable<EmployeeAttendance> GetForStatusAsQueryable(GetStatusBaseModel model);
    }
}
