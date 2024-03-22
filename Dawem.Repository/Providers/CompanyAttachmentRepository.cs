using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Generic;

namespace Dawem.Repository.Providers
{
    public class CompanyAttachmentRepository : GenericRepository<CompanyAttachment>, ICompanyAttachmentRepository
    {
        public CompanyAttachmentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
