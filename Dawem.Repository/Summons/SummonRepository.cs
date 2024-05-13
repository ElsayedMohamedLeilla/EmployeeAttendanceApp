using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Summons;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Summons.Summons;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Summons
{
    public class SummonRepository : GenericRepository<Summon>, ISummonRepository
    {
        private readonly RequestInfo _requestInfo;
        public SummonRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<Summon> GetAsQueryable(GetSummonsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Summon>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Summon>(true);
            var utcDate = DateTime.UtcNow;

            predicate = predicate.And(e => e.CompanyId == _requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.SummonSanctions.Any(a => a.Sanction.Name.ToLower().Trim().Contains(criteria.FreeText)));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Date != null)
            {
                predicate = predicate.And(e => e.LocalDateAndTime.Date == criteria.Date.Value.Date);
            }
            if (criteria.Status != null)
            {
                switch (criteria.Status.Value)
                {
                    case SummonStatus.NotStarted:
                        predicate = predicate.And(e => e.StartDateAndTimeUTC < utcDate);
                        break;
                    case SummonStatus.OnGoing:
                        predicate = predicate.And(e => utcDate >= e.StartDateAndTimeUTC && utcDate <= e.EndDateAndTimeUTC);
                        break;
                    case SummonStatus.Finished:
                        predicate = predicate.And(e => utcDate > e.EndDateAndTimeUTC);
                        break;
                    default:
                        break;
                }
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<Summon> EmployeeGetAsQueryable(GetSummonsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Summon>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Summon>(true);
            var innerFilterEmployee = PredicateBuilder.New<Summon>(true);
            var utcDate = DateTime.UtcNow;
            var employeeId = _requestInfo.EmployeeId;

            predicate = predicate.And(s => s.CompanyId == _requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.SummonSanctions.Any(a => a.Sanction.Name.ToLower().Trim().Contains(criteria.FreeText)));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Date != null)
            {
                predicate = predicate.And(e => e.LocalDateAndTime.Date == criteria.Date.Value.Date);
            }
            if (criteria.Status != null)
            {
                switch (criteria.Status.Value)
                {
                    case SummonStatus.NotStarted:
                        predicate = predicate.And(e => e.StartDateAndTimeUTC < utcDate);
                        break;
                    case SummonStatus.OnGoing:
                        predicate = predicate.And(e => utcDate >= e.StartDateAndTimeUTC && utcDate <= e.EndDateAndTimeUTC);
                        break;
                    case SummonStatus.Finished:
                        predicate = predicate.And(e => utcDate > e.EndDateAndTimeUTC);
                        break;
                    case SummonStatus.FinishedAndMissed:
                        predicate = predicate.And(e => utcDate > e.EndDateAndTimeUTC && !e.EmployeeAttendanceChecks.
                        Any(c => !c.IsDeleted && c.EmployeeAttendance.EmployeeId == employeeId && c.FingerPrintType == FingerPrintType.Summon));
                        break;
                    default:
                        break;
                }
            }

            innerFilterEmployee = innerFilterEmployee.And(s => s.ForAllEmployees.HasValue && s.ForAllEmployees.Value);

            innerFilterEmployee = innerFilterEmployee.Or(s =>
            s.SummonEmployees != null && s.SummonEmployees.Any(se => se.EmployeeId == _requestInfo.EmployeeId));

            innerFilterEmployee = innerFilterEmployee.Or(s =>
            s.SummonDepartments != null && s.SummonDepartments.Any(sd => sd.Department.Employees.Any(de => de.Id == _requestInfo.EmployeeId)));

            innerFilterEmployee = innerFilterEmployee.Or(s =>
            s.SummonGroups != null && s.SummonGroups.Any(sg => sg.Group.GroupEmployees.Any(ge => ge.EmployeeId == _requestInfo.EmployeeId)));

            predicate = predicate.And(inner).And(innerFilterEmployee);
            var Query = Get(predicate);
            return Query;

        }
    }
}
