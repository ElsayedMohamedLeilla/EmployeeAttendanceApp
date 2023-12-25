using Dawem.Domain.Entities.Providers;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface ICompanyBL
    {
        Task<Company> Create(Company company);
    }
}
