using Dawem.Models.Criteria.Others;

namespace Dawem.Contract.BusinessLogic.Others
{
    public interface IPermissionBL
    {
        Task<bool> CheckUserPermission(CheckUserPermissionModel model);
    }
}
