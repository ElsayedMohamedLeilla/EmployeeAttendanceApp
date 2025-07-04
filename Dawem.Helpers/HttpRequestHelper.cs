﻿using Dawem.Translations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Dawem.Helpers
{
    public static class HttpRequestHelper
    {
        public static string getLangKey(HttpRequest request)
        {
            var language = getHeaderKey<string>(request, LeillaKeys.Lang);
            if (string.IsNullOrEmpty(language)) language = getHeaderKey<string>(request, LeillaKeys.AcceptLanguage);
            if (string.IsNullOrEmpty(language)) language = LeillaKeys.Ar;
            return language;

        }

        public static T? getHeaderKey<T>(HttpRequest request, string key)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T?));
            var stringValue = request.Headers[key].FirstOrDefault();
            var headerKey = string.IsNullOrEmpty(stringValue) || string.IsNullOrWhiteSpace(stringValue) ? default : (T?)converter.ConvertFromString(stringValue);
            return headerKey;

        }

    }
}
