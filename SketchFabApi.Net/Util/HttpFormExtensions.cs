using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SketchFab
{
    public static class HttpFormExtensions
    {
        public static string To_0_1(this bool value)
        {
            return value ? "1" : "0";
        }


        public static void AddRange(this MultipartFormDataContent form, IEnumerable<string> values, string name)
        {
            if (values == null) return;

            foreach (var value in values)
            {
                form.Add(new StringContent(value), name);
            }
        }

        public static void AddAuthorizationHeader(this HttpRequestMessage message, string token, TokenType tokenType)
        {
            message.Headers.Add("Authorization", $"{tokenType.ToString()} {token}");
        }


    }
}
