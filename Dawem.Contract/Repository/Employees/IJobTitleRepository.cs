using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.JobTitle;

namespace Dawem.Contract.Repository.Employees
{
    public interface IJobTitleRepository : IGenericRepository<JobTitle>
    {
        IQueryable<JobTitle> GetAsQueryable(GetJobTitlesCriteria criteria);
    }
}
