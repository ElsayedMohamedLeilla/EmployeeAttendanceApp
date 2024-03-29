using Dawem.Models.Dtos.Dawem.Providers.Companies;

namespace Dawem.Contract.BusinessValidation.Dawem.Providers
{
    public interface ICompanyBLValidation
    {
        Task<bool> CreateValidation(CreateCompanyModel model);
        Task<bool> UpdateValidation(UpdateCompanyModel model);
    }
}
