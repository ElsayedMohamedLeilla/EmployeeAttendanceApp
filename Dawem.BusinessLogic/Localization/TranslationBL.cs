

using SmartBusinessERP.Repository.Localization.Contract;
using SmartBusinessERP.BusinessLogic.Localization.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Dtos.Shared;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Localization
{
    public class TranslationBL : ITranslationBL
    {

        private readonly ITranslationRepository _translationRepository;
        private readonly IUnitOfWork<ApplicationDBContext> _unitOfWork;
        public TranslationBL(IUnitOfWork<ApplicationDBContext> unitOfWork, ITranslationRepository translationRepository)
        {
            _unitOfWork = unitOfWork;
            _translationRepository = translationRepository;
        }

        public BaseResponseT<bool> RefreshCachedTranslation()
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {
                var query = _translationRepository.Get();
                var translations = query.ToList();
                if (translations != null)
                {
                    var ar = translations.FindAll(c => c.Lang == "ar");
                    if (ar.Count > 0)
                    {
                        TranslationHelper.setArTrans(ar.Select(x => new TransModel { KeyWord = x.KeyWord, TransWords = x.TransWords }));
                    }

                    var en = translations.FindAll(c => c.Lang == "en");
                    if (en.Count > 0)
                    {
                        TranslationHelper.setEnTrans(en.Select(x => new TransModel { KeyWord = x.KeyWord, TransWords = x.TransWords }));
                    }
                }
                response.Result = true;
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }


    }
}
