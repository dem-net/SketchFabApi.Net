//
// SketchfabApi.Account.cs
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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sketchfab;

public partial class SketchfabApi
{
    public async Task<Account> GetMyAccountAsync(string sketchFabToken, TokenType tokenType, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get account");

        using var request = CreateRequest(HttpMethod.Get, $"{SketchfabApiUrl}/me/account", sketchFabToken, tokenType);
        return await SendForJsonAsync(request, SketchfabJsonContext.Default.Account, nameof(GetMyAccountAsync), cancellationToken).ConfigureAwait(false);
    }

    public async Task PostLikeAsync(string modelId, string sketchFabToken, TokenType tokenType, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelId);
        _logger.LogInformation("Like model {ModelId}", modelId);

        using var form = new MultipartFormDataContent
        {
            { new StringContent(modelId), "model" }
        };
        using var request = CreateRequest(HttpMethod.Post, $"{SketchfabApiUrl}/me/likes", sketchFabToken, tokenType);
        request.Content = form;

        await SendAsync(request, nameof(PostLikeAsync), cancellationToken).ConfigureAwait(false);
    }

    [Obsolete("Use PostLikeAsync.")]
#pragma warning disable VSTHRD200 // kept for source compatibility
    public Task PostLike(string modelId, string sketchFabToken, TokenType tokenType)
        => PostLikeAsync(modelId, sketchFabToken, tokenType);
#pragma warning restore VSTHRD200
}
