using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Dawem;
using Dawem.Models.Generic;

namespace Dawem.Repository.Providers
{
    public class DawemSettingRepository : GenericRepository<DawemSetting>, IDawemSettingRepository
    {
        public DawemSettingRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
