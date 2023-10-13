using Dawem.Contract.BusinessLogic.Localization;
using Dawem.Contract.Repository.Localization;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Response;

namespace Dawem.BusinessLogic.Localization
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
                        TranslationHelper.SetArTrans(ar.Select(x => new TransModel { KeyWord = x.KeyWord, TransWords = x.TransWords }));
                    }

                    var en = translations.FindAll(c => c.Lang == "en");
                    if (en.Count > 0)
                    {
                        TranslationHelper.SetEnTrans(en.Select(x => new TransModel { KeyWord = x.KeyWord, TransWords = x.TransWords }));
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
