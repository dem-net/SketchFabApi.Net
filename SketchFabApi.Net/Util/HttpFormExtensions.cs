using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Sketchfab
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

            if (values.Any())
            {
                foreach (var value in values)
                {
                    form.Add(new StringContent(value), name);
                }
            }
            else
            {
                form.Add(new StringContent(string.Empty), name);
            }
        }

        public static void AddAuthorizationHeader(this HttpRequestMessage message, string token, TokenType tokenType)
        {
            message.Headers.Add("Authorization", $"{tokenType.ToString()} {token}");
        }

        public static HttpRequestMessage Clone(this HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            clone.Content = req.Content;
            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }


    }
}
