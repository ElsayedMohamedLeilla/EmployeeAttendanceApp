

using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Localization.Contract
{
    public interface ITranslationBL
    
    {
      
        BaseResponseT<bool> RefreshCachedTranslation();
     
    

   }
}