using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dashboard;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Requests
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        private readonly RequestInfo requestInfo;
        public RequestRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<Request> GetAsQueryable(GetRequestsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Request>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Request>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Employee != null && x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                
                inner = inner.Or(x => x.RequestAssignment.AssignmentType != null && x.RequestAssignment.AssignmentType.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.RequestJustification.JustificatioType != null && x.RequestJustification.JustificatioType.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.RequestPermission.PermissionType != null && x.RequestPermission.PermissionType.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.RequestTask.TaskType != null && x.RequestTask.TaskType.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.RequestVacation.VacationType != null && x.RequestVacation.VacationType.Name.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(request => request.CompanyId == requestInfo.CompanyId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.Type is not null)
            {
                predicate = predicate.And(e => e.Type == criteria.Type);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(e => e.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(e => e.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(e => e.Date == criteria.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<Request> EmployeeGetAsQueryable(EmployeeGetRequestsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Request>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Request>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Employee != null && x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(request => request.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(request => request.EmployeeId == requestInfo.EmployeeId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.Type is not null)
            {
                predicate = predicate.And(e => e.Type == criteria.Type);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(e => e.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(e => e.Date == criteria.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<Request> GetForStatusAsQueryable(GetStatusBaseModel model)
        {
            var predicate = PredicateBuilder.New<Request>(a => !a.IsDeleted);

            predicate = predicate.And(request => request.CompanyId == requestInfo.CompanyId);

            if (model.Type != null)
            {
                switch (model.Type)
                {
                    case GetRequestsStatusType.CurrentDay:
                        predicate = predicate.And(request => request.Date.Date == model.LocalDate.Date);
                        break;
                    case GetRequestsStatusType.CurrentMonth:
                        predicate = predicate.And(request => request.Date.Month == model.LocalDate.Month && request.Date.Year == model.LocalDate.Year);
                        break;
                    case GetRequestsStatusType.CurrentYear:
                        predicate = predicate.And(request => request.Date.Year == model.LocalDate.Year);
                        break;
                    default:
                        break;
                }
            }
            if (model.DateFrom != null)
            {
                predicate = predicate.And(request => request.Date >= model.DateFrom.Value);
            }
            if (model.DateTo != null)
            {
                predicate = predicate.And(request => request.Date <= model.DateTo.Value);
            }
            var Query = Get(predicate);
            return Query;
        }
    }
}
