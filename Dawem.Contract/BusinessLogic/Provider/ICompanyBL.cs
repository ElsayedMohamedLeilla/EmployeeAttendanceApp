using Dawem.Domain.Entities.Provider;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface ICompanyBL
    {
        Task<Company> Create(Company company);
    }
}
