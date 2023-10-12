using SmartBusinessERP.BusinessLogic.Provider.Contract;
using SmartBusinessERP.Domain.Entities.Packages;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Provider
{
    public class PackageScreenBL : IPackageScreenBL
    {
        public BaseResponseT<List<PackageScreen>> BulkCreate(List<PackageScreen> packageScreen)
        {
            throw new NotImplementedException();
        }

        public BaseResponseT<PackageScreen> Create(PackageScreen packageScreen)
        {
            throw new NotImplementedException();
        }

        public BaseResponseT<bool> Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public BaseResponseT<bool> Update(PackageScreen packageScreen)
        {
            throw new NotImplementedException();
        }
    }
}
