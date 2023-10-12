using SmartBusinessERP.Domain.Entities.Inventory;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Dtos.Shared;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;

namespace SmartBusinessERP.BusinessLogic.Core.Contract
{
    public interface IUnitBL
    {
        Task<BaseResponseT<UnitDTO>> GetById(int Id);
        Task<UnitSearchResult> Get(UnitSearchCriteria criteria);
        Task<GetUnitInfoResponse> GetInfo(GetUnitInfoCriteria criteria);
        Task<BaseResponseT<Unit>> Create(Unit unit);
        Task<BaseResponseT<bool>> Update(Unit unit);
        Task<BaseResponseT<bool>> Delete(int Id);
        BaseResponseT<bool> IsNameUnique(ValidationItems validationItem);
    }
}
