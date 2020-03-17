using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DEM.Net.Extension.SketchFab
{
    public partial class SketchFabApi
    {

        public async Task<List<Collection>> GetMyCollectionsAsync()
        {
            try
            {
                _logger.LogInformation($"Get collections");

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{SketchFabApiUrl}/me/collections ");
                httpRequestMessage.Headers.Add("Authorization", $"Token {_secrets.SketchFabToken}");
               
                var response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead);
                _logger.LogInformation($"{nameof(GetMyCollectionsAsync)} responded {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var collectionsJson = await response.Content.ReadAsStringAsync();

                var collections = JsonConvert.DeserializeObject<PagedResult<Collection>>(collectionsJson);

                _logger.LogInformation($"GetMyCollectionsAsync got {collections.results.Count} collection(s).");

                return collections.results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SketchFab upload error: {ex.Message}");
                throw;
            }

        }

    }
}
