//
// SketchfabApi.Collections.cs
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sketchfab
{


    public partial class SketchfabApi
    {

        public async Task<List<Collection>> GetMyCollectionsAsync(string sketchFabToken, TokenType tokenType)
        {
            try
            {
                _logger.LogInformation($"Get collections");

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{SketchfabApiUrl}/me/collections");
                httpRequestMessage.AddAuthorizationHeader(sketchFabToken, tokenType);
                
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead);
                _logger.LogInformation($"{nameof(GetMyCollectionsAsync)} responded {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var collectionsJson = await response.Content.ReadAsStringAsync();

                var collections = JsonConvert.DeserializeObject<PagedResult<Collection>>(collectionsJson);

                _logger.LogInformation($"GetMyCollectionsAsync got {collections.results.Count} collection(s).");

                return collections.results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Sketchfab upload error: {ex.Message}");
                throw;
            }

        }

        public async Task AddModelToCollectionAsync(string collectionId, string sketchFabToken, TokenType tokenType, params string[] modelIds)
        {
            try
            {
                _logger.LogInformation($"Add model(s) to collection");
                if (modelIds == null || modelIds.Length == 0) throw new ArgumentNullException(nameof(modelIds));
                if (string.IsNullOrWhiteSpace(collectionId)) throw new ArgumentNullException(nameof(collectionId));

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{SketchfabApiUrl}/collections/{collectionId}/models");
                httpRequestMessage.AddAuthorizationHeader(sketchFabToken, tokenType);

                using var form = new MultipartFormDataContent();
                form.Headers.ContentType.MediaType = "multipart/form-data";

                form.AddRange(modelIds, "models");

                httpRequestMessage.Content = form;

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.SendAsync(httpRequestMessage);

                _logger.LogInformation($"{nameof(AddModelToCollectionAsync)} responded {response.StatusCode}");
                response.EnsureSuccessStatusCode();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Sketchfab add model(s) to collection error: {ex.Message}");
                throw;
            }

        }

    }
}
