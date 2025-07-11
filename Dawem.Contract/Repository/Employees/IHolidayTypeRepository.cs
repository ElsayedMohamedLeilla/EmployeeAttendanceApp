﻿using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;

namespace Dawem.Contract.Repository.Employees
{
    public interface IHolidayTypeRepository : IGenericRepository<HolidayType>
    {
        IQueryable<HolidayType> GetAsQueryable(GetHolidayTypesCriteria criteria);
    }
}
