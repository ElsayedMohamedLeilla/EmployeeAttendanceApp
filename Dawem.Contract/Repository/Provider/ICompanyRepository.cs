﻿using Dawem.Data;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Dtos.Dawem.Providers.Companies;

namespace Dawem.Contract.Repository.Provider
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        IQueryable<Company> GetAsQueryable(GetCompaniesCriteria criteria);
    }
}
