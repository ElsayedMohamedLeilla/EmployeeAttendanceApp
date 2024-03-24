using Dawem.Models.Dtos.Providers.Companies;

namespace Dawem.Contract.BusinessValidation.Providers
{
    public interface ICompanyBLValidation
    {
        Task<bool> CreateValidation(CreateCompanyModel model);
        Task<bool> UpdateValidation(UpdateCompanyModel model);
    }
}
