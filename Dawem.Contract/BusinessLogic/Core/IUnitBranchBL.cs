using SmartBusinessERP.Domain.Entities.Inventory;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Core.Contract
{
    public interface IUnitBranchBL
    {
     
        BaseResponseT<List<UnitBranchDTO>> GetByUnit(int unitId);
        BaseResponseT<UnitBranchDTO> Get(int Id);
        BaseResponseT<List<UnitBranchDTO>> Get();
        BaseResponseT<UnitBranch> Create(UnitBranch unitBranch);
        BaseResponseT<bool> Update(UnitBranch unitBranch);
        BaseResponseT<bool> Delete(int Id);
        BaseResponseT<List<UnitBranch>> BulkCreate(List<UnitBranch> UnitBrancheList);
    }
}
