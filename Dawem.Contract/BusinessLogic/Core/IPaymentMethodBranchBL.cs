using SmartBusinessERP.Domain.Entities.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Core.Contract
{
    public interface IPaymentMethodBranchBL
    {
        BaseResponseT<List<PaymentMethodBranchDTO>> GetByPaymentMethod(int paymentMethodId);
      
        BaseResponseT<PaymentMethodBranch> Create(PaymentMethodBranch paymentMethodBranche);
     
        BaseResponseT<List<PaymentMethodBranch>> BulkCreate(List<PaymentMethodBranch> PaymentMethodBrancheList);
    }
}
