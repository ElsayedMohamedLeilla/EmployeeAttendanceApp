using SmartBusinessERP.Domain.Entities.Packages;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IPackageScreenBL 
    {
        BaseResponseT<PackageScreen> Create(PackageScreen packageScreen);

        BaseResponseT<List<PackageScreen>> BulkCreate(List<PackageScreen> packageScreen);

        BaseResponseT<bool> Delete(int Id);

        BaseResponseT<bool> Update(PackageScreen packageScreen);
    }
}
