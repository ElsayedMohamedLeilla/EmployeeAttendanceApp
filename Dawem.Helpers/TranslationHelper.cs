using Dawem.Models.Dtos.Shared;
using Dawem.Translations;
using System.Runtime.Caching;

namespace Dawem.Helpers
{
    public class TranslationHelper
    {

        public static MemoryCache memCache = MemoryCache.Default;
        public static T? GetCachedData<T>(string key)
        {
            try
            {
                if (memCache.Contains(key))
                    return (T)memCache.Get(key);
                return default;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        public static string GetTranslation(string key, string lang)
        {
            string cachedKey = LeillaKeys.ArTrans;
            if (lang == LeillaKeys.En)
            {
                cachedKey = LeillaKeys.EnTrans;
            }
            var cached = GetCachedData<IEnumerable<TransModel>>(cachedKey);
            if (cached != null)
            {
                List<TransModel> data = cached.ToList();
                if (data != null)
                {
                    var tr = data.Find(x => x.KeyWord == key);
                    if (tr != null)
                    {
                        return tr.TransWords;
                    }
                    else
                    {
                        return key;
                    }
                }
                else
                {
                    return key;
                }
            }
            return key;
        }
        public static void SetlangTrans(object translations, string cachKey)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(365)
            };
            if (memCache.Contains(cachKey))
            {
                memCache.Remove(cachKey);
            }
            memCache.Add(cachKey, translations, cacheItemPolicy);
        }
        public static void SetArTrans(object translations)
        {
            SetlangTrans(translations, LeillaKeys.ArTrans);
        }
        public static void SetEnTrans(object translations)
        {
            SetlangTrans(translations, LeillaKeys.EnTrans);
        }
        public static List<MetaPair> GetResponseMessages(string messageCode,
           string message, string MetaPlus = LeillaKeys.EmptyString)
        {
            return new List<MetaPair>() { new MetaPair() { Property = messageCode,
                Meta = message, MetaPlus = MetaPlus } };

        }

        public static void LocalizeMessage(ref string messageCode, ref string message, string lang)
        {
            string _keyWord = messageCode;
            var transaltedMessage = GetTranslation(_keyWord, lang);
            message = transaltedMessage == _keyWord ? message : transaltedMessage;
        }


    }
}
