using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.Requests.Vacations;
using LinqKit;

namespace Dawem.Repository.Requests
{
    public class RequestVacationRepository : GenericRepository<RequestVacation>, IRequestVacationRepository
    {
        private readonly RequestInfo requestInfo;
        public RequestVacationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<RequestVacation> GetAsQueryable(GetRequestVacationsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestVacation>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestVacation>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.VacationType != null && x.VacationType.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestVacation => requestVacation.Request.CompanyId == requestInfo.CompanyId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestVacation => criteria.Ids.Contains(requestVacation.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Code == criteria.Code);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.VacationTypeId is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.VacationTypeId == criteria.VacationTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestVacation => criteria.Date.Value.Date >= requestVacation.Request.Date.Date && criteria.Date.Value.Date <= requestVacation.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<RequestVacation> EmployeeGetAsQueryable(EmployeeGetRequestVacationsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestVacation>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestVacation>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.VacationType != null && x.VacationType.Name.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestVacation => requestVacation.Request.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(requestVacation => requestVacation.Request.EmployeeId == requestInfo.EmployeeId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestVacation => criteria.Ids.Contains(requestVacation.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Code == criteria.Code);
            }
            if (criteria.VacationTypeId is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.VacationTypeId == criteria.VacationTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestVacation => criteria.Date.Value.Date >= requestVacation.Request.Date.Date && criteria.Date.Value.Date <= requestVacation.DateTo.Date);
            }

            var localDateTime = requestInfo.LocalDateTime;
            switch (criteria.VacationStatus)
            {
                case VacationStatus.Previous:
                    predicate = predicate.And(requestVacation => (requestVacation.Request.Date.Month < localDateTime.Month &&
                    requestVacation.Request.Date.Year <= localDateTime.Year) ||
                    requestVacation.Request.Date.Year < localDateTime.Year ||
                    (requestVacation.DateTo.Month < localDateTime.Month &&
                    requestVacation.DateTo.Year <= localDateTime.Year) ||
                    requestVacation.DateTo.Year < localDateTime.Year);
                    break;
                case VacationStatus.Current:
                    predicate = predicate.And(requestVacation => (requestVacation.Request.Date.Month == localDateTime.Month &&
                    requestVacation.Request.Date.Year == localDateTime.Year) || 
                    (requestVacation.DateTo.Month == localDateTime.Month && 
                    requestVacation.DateTo.Year == localDateTime.Year));
                    break;     
                case VacationStatus.Next:
                    predicate = predicate.And(requestVacation => (requestVacation.Request.Date.Month >localDateTime.Month &&
                    requestVacation.Request.Date.Year >= localDateTime.Year) ||
                    requestVacation.Request.Date.Year > localDateTime.Year ||
                    (requestVacation.DateTo.Month > localDateTime.Month &&
                    requestVacation.DateTo.Year >= localDateTime.Year) ||
                    requestVacation.DateTo.Year > localDateTime.Year);
                    break;
                default:
                    break;
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
