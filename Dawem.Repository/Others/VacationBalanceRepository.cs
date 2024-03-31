using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Others.VacationBalances;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Others
{
    public class VacationBalanceRepository : GenericRepository<VacationBalance>, IVacationBalanceRepository
    {
        private readonly RequestInfo _requestInfo;
        public VacationBalanceRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<VacationBalance> GetAsQueryable(GetVacationBalancesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<VacationBalance>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<VacationBalance>(true);

            predicate = predicate.And(e => e.CompanyId == _requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Or(x => x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));

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

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
