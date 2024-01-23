using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Core.Holidays
{
    public class HolidayRepository : GenericRepository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }

        public IQueryable<Holiday> GetAsQueryable(GetHolidayCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Holiday>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Holiday>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));
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
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
            }
            //// search by year get all year is zero or year = critraia.year
            //if (criteria.Year > 0 || criteria.Year != null)
            //{
            //    inner = inner.And(e => e.StartYear == criteria.Year || e.StartYear == null);

            //}
            if (criteria.DateType != null)
            {
                inner = inner.And(e => e.DateType == criteria.DateType);
            }



            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
