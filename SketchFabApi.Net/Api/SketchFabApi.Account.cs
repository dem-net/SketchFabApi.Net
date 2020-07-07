//
// SketchFabApi.Account.cs
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

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SketchFab;

namespace SketchFab
{
    public partial class SketchFabApi
    {
        public async Task<Account> GetMyAccountAsync(string sketchFabToken, TokenType tokenType)
        {
            try
            {
                _logger.LogInformation($"Get Account");

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{SketchFabApiUrl}/me/account");
                httpRequestMessage.AddAuthorizationHeader(sketchFabToken, tokenType);

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead);
                _logger.LogInformation($"{nameof(GetMyAccountAsync)} responded {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var jsonPayload = await response.Content.ReadAsStringAsync();

                var account = JsonConvert.DeserializeObject<Account>(jsonPayload);

                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SketchFab get account error: {ex.Message}");
                throw;
            }

        }
    }
}
