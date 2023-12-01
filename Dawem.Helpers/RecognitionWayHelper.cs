using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Translations;

namespace Dawem.Helpers
{
    public  class RecognitionWayHelper

    {
        private readonly RequestInfo requestInfo;
        public RecognitionWayHelper(RequestInfo _requestHeaderContext)
        {
            requestInfo = _requestHeaderContext;
        }
       

        public  string GetWayOfRecognition(RecognitionWay MinCheckinRecognitionWay, RecognitionWay MaxCheckOutRecognitionWay)
        {
            string checkInMethod = GetRecognitionMethodName(MinCheckinRecognitionWay);
            string checkOutMethod = GetRecognitionMethodName(MaxCheckOutRecognitionWay);

            if (checkInMethod == checkOutMethod)
            {
                return checkInMethod;
            }
            else
            {
                return checkInMethod + " / " + checkOutMethod;
            }
        }
        private  string GetRecognitionMethodName(RecognitionWay way)
        {
            switch (way)
            {
                case RecognitionWay.FaceRecognition:
                    return TranslationHelper.GetTranslation(AmgadKeys.FaceRecognition, requestInfo.Lang);
                case RecognitionWay.FingerPrint:
                    return TranslationHelper.GetTranslation(AmgadKeys.FingerPrint, requestInfo.Lang);
                case RecognitionWay.VoiceRecognition:
                    return TranslationHelper.GetTranslation(AmgadKeys.VoiceRecognition, requestInfo.Lang);
                case RecognitionWay.PinRecognition:
                    return TranslationHelper.GetTranslation(AmgadKeys.PinRecognition, requestInfo.Lang);
                case RecognitionWay.PaternRecognition:
                    return TranslationHelper.GetTranslation(AmgadKeys.PaternRecognition, requestInfo.Lang);
                case RecognitionWay.PasswordRecognition:
                    return TranslationHelper.GetTranslation(AmgadKeys.PasswordRecognition, requestInfo.Lang);
                default:
                    return TranslationHelper.GetTranslation(AmgadKeys.Unknown, requestInfo.Lang);
            }
        }
    }
}
