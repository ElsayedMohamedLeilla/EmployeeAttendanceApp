using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Repository.Core.Conract;

namespace Dawem.Repository.Core
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        private readonly RequestHeaderContext userContext;
        public GroupRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestHeaderContext _userContext) : base(unitOfWork)
        {
            userContext = _userContext;
        }
        //public IQueryable<Group> GetAsQueryable(GetGroupsCriteria criteria, string includeProperties = DawemKeys.EmptyString)
        //{
        //    var outerpredicate = PredicateBuilder.New<Group>(true);

        //    var inner = PredicateBuilder.New<Group>(true);

        //    outerpredicate = outerpredicate.And(x => x.MainBranchId == userContext.BranchId);




        //    if (!string.IsNullOrWhiteSpace(criteria.FreeText))
        //    {
        //        criteria.FreeText = criteria.FreeText.ToLower().Trim();

        //        inner = inner.Start(x => x.NameEn.ToLower().Contains(criteria.FreeText));

        //        inner = inner.Or(x => x.NameAr.ToLower().Contains(criteria.FreeText));
        //        if (int.TryParse(criteria.FreeText, out int id))
        //        {
        //            inner = inner.Or(x => x.Id == id);
        //        }
        //    }
        //    if (criteria.Id != null)
        //    {
        //        outerpredicate = outerpredicate.And(x => x.Id == criteria.Id.Value);
        //    }


        //    outerpredicate = outerpredicate.And(inner);
        //    var query = Get(outerpredicate, includeProperties: includeProperties);
        //    return query;
        //}

    }

}
