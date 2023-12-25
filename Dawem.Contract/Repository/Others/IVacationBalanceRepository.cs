using Dawem.Data;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Dtos.Others.VacationBalances;

namespace Dawem.Contract.Repository.Others
{
    public interface IVacationBalanceRepository : IGenericRepository<VacationBalance>
    {
        IQueryable<VacationBalance> GetAsQueryable(GetVacationBalancesCriteria criteria);
    }
}
