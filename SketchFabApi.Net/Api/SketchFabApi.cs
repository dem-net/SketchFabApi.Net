//
// SketchfabApi.cs
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
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sketchfab;

/// <summary>
/// Client for the Sketchfab Data API v3. Register it with <c>services.AddSketchfabApi()</c>.
/// You will find your Sketchfab token at https://sketchfab.com/settings/password
/// </summary>
/// <remarks>
/// Uploading a model to Sketchfab is a two step process:
/// <list type="number">
/// <item>Upload a model. If the upload is successful, the API returns the model's uid in the <c>Location</c> header
/// and the model is placed in the processing queue.</item>
/// <item>Poll for the processing status (PENDING, PROCESSING, SUCCEEDED or FAILED) with the model uid.
/// Limit the rate at which you poll: once every few seconds is more than enough.</item>
/// </list>
/// </remarks>
public partial class SketchfabApi(ILogger<SketchfabApi> logger, IHttpClientFactory httpClientFactory)
{
    /// <summary>
    /// Name of the <see cref="HttpClient"/> requested from the <see cref="IHttpClientFactory"/>, configured by <c>AddSketchfabApi()</c>.
    /// </summary>
    public const string HttpClientName = "Sketchfab";

    private const string SketchfabApiUrl = "https://api.sketchfab.com/v3";
    private const int MaxLoggedErrorBodyLength = 500;

    private readonly ILogger<SketchfabApi> _logger = logger;

    private HttpClient CreateClient() => httpClientFactory.CreateClient(HttpClientName);

    private static HttpRequestMessage CreateRequest(HttpMethod method, string url, string token, TokenType tokenType)
    {
        var request = new HttpRequestMessage(method, url);
        request.AddAuthorizationHeader(token, tokenType);
        return request;
    }

    /// <summary>
    /// Sends the request and fails on a non-success status code. The response body is logged on failure.
    /// </summary>
    private async Task<HttpResponseMessage> SendCheckedAsync(HttpRequestMessage request, string operation, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await CreateClient().SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Sketchfab {Operation} failed", operation);
            throw;
        }

        _logger.LogInformation("Sketchfab {Operation} responded {StatusCode}", operation, response.StatusCode);
        if (!response.IsSuccessStatusCode)
        {
            using (response)
            {
                await LogErrorResponseAsync(response, operation, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
        }
        return response;
    }

    private async Task SendAsync(HttpRequestMessage request, string operation, CancellationToken cancellationToken)
    {
        using var response = await SendCheckedAsync(request, operation, cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> SendForJsonAsync<T>(HttpRequestMessage request, JsonTypeInfo<T> typeInfo, string operation, CancellationToken cancellationToken)
    {
        using var response = await SendCheckedAsync(request, operation, cancellationToken).ConfigureAwait(false);
        try
        {
            var result = await response.Content.ReadFromJsonAsync(typeInfo, cancellationToken).ConfigureAwait(false);
            return result ?? throw new InvalidDataException($"Sketchfab {operation} returned an empty response.");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Sketchfab {Operation} returned an unexpected response", operation);
            throw;
        }
    }

    private async Task LogErrorResponseAsync(HttpResponseMessage response, string operation, CancellationToken cancellationToken)
    {
        string body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (body.Length > MaxLoggedErrorBodyLength)
        {
            body = body[..MaxLoggedErrorBodyLength] + "...";
        }
        _logger.LogError("Sketchfab {Operation} error: {StatusCode} {ReasonPhrase} {Body}", operation, (int)response.StatusCode, response.ReasonPhrase, body);
    }

    private static string ShortenString(string? str, int length)
        => str is null ? string.Empty : str[..Math.Min(str.Length, length)];

    private static void AddCommonModelFields(MultipartFormDataContent form, UploadModelRequest request)
    {
        form.Add(new StringContent(ShortenString(request.Name, 48)), "name");
        form.Add(new StringContent(ShortenString(request.Description, 1024)), "description");
        form.Add(new StringContent(request.IsPrivate.To_0_1()), "private");
        form.Add(new StringContent(request.IsInspectable.To_0_1()), "isInspectable");
        form.Add(new StringContent(request.IsPublished.To_0_1()), "isPublished");
        form.Add(new StringContent(request.License), "license");
        form.Add(new StringContent(JsonSerializer.Serialize(request.Options, SketchfabJsonContext.Default.ModelOptions)), "options");
        form.AddRange(request.Tags, "tags");
        form.AddRange(request.Categories, "categories");
    }

    private static void AddCommonModelFields(MultipartFormDataContent form, UpdateModelRequest request)
    {
        form.AddRange(request.Tags, "tags");
        form.AddRange(request.Categories, "categories");
    }
}
