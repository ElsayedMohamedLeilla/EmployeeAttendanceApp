using SmartBusinessERP.Domain.Entities.Inventory;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;

namespace SmartBusinessERP.BusinessLogic.Core.Contract
{
    public interface IProductUnitBL
    {
        BaseResponseT<ProductUnitDto> GetById(int Id);
        ProductUnitSearchResult Get(ProductUnitSearchCriteria criteria);
        BaseResponseT<float> GetUnitRate(int unitId, int productId);
        BaseResponseT<ItemUnit> Create(ItemUnit productUnit);
        BaseResponseT<bool> Update(ItemUnit productUnit);
        BaseResponseT<bool> Delete(int Id);
        BaseResponseT<List<ProductUnitDto>> GetByProduct(int productId);
    }
}
