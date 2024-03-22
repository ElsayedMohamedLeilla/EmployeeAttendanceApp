using Dawem.Data;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.Repository.Provider
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        IQueryable<Company> GetAsQueryable(GetCompaniesCriteria criteria);
    }
}
