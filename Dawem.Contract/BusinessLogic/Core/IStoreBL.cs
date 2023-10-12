using SmartBusinessERP.Domain.Entities.Inventory;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;

namespace SmartBusinessERP.BusinessLogic.Core.Contract
{
    public interface IStoreBL
    {
        Task<BaseResponseT<StoreDTO>> GetById(int Id);
        Task<GetStoresResponse> Get(GetStoresCriteria criteria);
        Task<GetStoreInfoResponse> GetInfo(GetStoreInfoCriteria criteria);
        Task<BaseResponseT<Store>> Create(Store store);
        Task<BaseResponseT<bool>> Update(Store store);
        Task<BaseResponseT<bool>> Delete(int Id);
    }
}
