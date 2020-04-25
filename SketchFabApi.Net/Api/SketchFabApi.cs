//
// SketchFabApi.cs
//
// Author:
//       Xavier Fischer 2020-4
//
// Copyright (c) 2020 Xavier Fischer
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;

namespace SketchFab
{

    /// <summary>
    /// You will find your SketchFab token at https://sketchfab.com/settings/password
    /// </summary>
    public partial class SketchFabApi
    {
        private readonly ILogger<SketchFabApi> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private const string SketchFabApiUrl = "https://api.sketchfab.com/v3";
        private readonly DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };


        /*
        # Uploading a model to Sketchfab is a two step process
        #
        # 1. Upload a model. If the upload is successful, the API will return
        # the model's uid in the `Location` header, and the model will be placed in the processing queue
        #
        # 2. Poll for the processing status
        # You can use your model id (see 1.) to poll the model processing status
        # The processing status can be one of the following:
        #    - PENDING: the model is in the processing queue
        #    - PROCESSING: the model is being processed
        #    - SUCCESSED: the model has being sucessfully processed and can be view on sketchfab.com
        #    - FAILED: the processing has failed. An error message detailing the reason for the failure
        # will be returned with the response
        #
        # HINTS
        # - limit the rate at which you poll for the status (once every few seconds is more than enough)
        */
        public SketchFabApi(ILogger<SketchFabApi> logger)
        {
            this._logger = logger;
            this._httpClient = new HttpClient();
            this._jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            JsonConvert.DefaultSettings = () => _jsonSerializerSettings;

        }

        private string ShortenString(string str, int length)
        {
            return str.Substring(0, Math.Min(str.Length, length));
        }
        private void AddCommonModelFields(MultipartFormDataContent form, UploadModelRequest request)
        {
            form.Add(new StringContent(ShortenString(request.Name, 48)), "name");
            form.Add(new StringContent(ShortenString(request.Description, 1024)), "description");
            form.Add(new StringContent(request.IsPrivate.To_0_1()), "private");
            form.Add(new StringContent(request.IsInspectable.To_0_1()), "isInspectable");
            form.Add(new StringContent(request.IsPublished.To_0_1()), "isPublished");
            form.Add(new StringContent(request.License), "license");
            var optionsJson = JsonConvert.SerializeObject(request.Options);
            form.Add(new StringContent(JsonConvert.SerializeObject(request.Options)), "options");
            form.AddRange(request.Tags, "tags");
            form.AddRange(request.Categories, "categories");
        }
        private void AddCommonModelFields(MultipartFormDataContent form, UpdateModelRequest request)
        {
            form.AddRange(request.Tags, "tags");
            form.AddRange(request.Categories, "categories");
        }

    }
}
