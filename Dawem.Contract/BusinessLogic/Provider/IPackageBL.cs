using SmartBusinessERP.Domain.Entities.Packages;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Dtos.Provider;
using SmartBusinessERP.Models.Criteria.Provider;
using SmartBusinessERP.Models.Response.Provider;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IPackageBL 
    {
         BaseResponseT<Package> Create(PackageDto package);

 
       
        BaseResponseT<bool> Delete(int Id);

      
        PackageSearchResult Get(PackageCriteria criteria);

       
        BaseResponseT<PackageDto> GetById(int Id);
        BaseResponseT<bool> Update(Package package);

     
    }
}
