using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultDepartmentsBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultDepartmentsDTO model);
        Task<bool> UpdateValidation(UpdateDefaultDepartmentsDTO model);
    }
}
