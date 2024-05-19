using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
            
        }
        public IQueryable<Company> GetAsQueryable(GetCompaniesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Company>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Company>(true);


            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Email.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.HeadquarterAddress.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.HeadquarterPostalCode.ToLower().Trim().StartsWith(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
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
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.SubscriptionType is not null)
            {
                switch (criteria.SubscriptionType.Value)
                {
                    case SubscriptionType.Subscription:
                        predicate = predicate.And(e => !e.Subscription.Plan.IsTrial);
                        break;
                    case SubscriptionType.Trial:
                        predicate = predicate.And(e => e.Subscription.Plan.IsTrial);
                        break;
                    default:
                        break;
                }
            }
            if (criteria.NumberOfEmployeesFrom is not null)
            {
                predicate = predicate.And(e => e.NumberOfEmployees >= criteria.NumberOfEmployeesFrom);
            }
            if (criteria.NumberOfEmployeesTo is not null)
            {
                predicate = predicate.And(e => e.NumberOfEmployees <= criteria.NumberOfEmployeesTo);
            }
            if (criteria.CountryId is not null)
            {
                predicate = predicate.And(e => e.CountryId <= criteria.CountryId);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
