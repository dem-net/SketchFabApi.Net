using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DEM.Net.Extension
{
    public static class HttpFormExtensions
    {
        public static string To_0_1(this bool value)
        {
            return value ? "1" : "0";
        }


        public static void AddRange(this MultipartFormDataContent form, List<string> values, string name)
        {
            if (values == null) return;

            foreach (var value in values)
            {
                form.Add(new StringContent(value), name);
            }
        }


    }
}
