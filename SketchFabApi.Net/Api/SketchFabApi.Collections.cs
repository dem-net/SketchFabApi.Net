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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sketchfab;

public partial class SketchfabApi
{
    /// <summary>
    /// Gets all the collections of the token owner (all pages).
    /// </summary>
    public async Task<List<Collection>> GetMyCollectionsAsync(string sketchFabToken, TokenType tokenType, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Get collections");

        var collections = new List<Collection>();
        await foreach (var collection in GetPagedResultAsync($"{SketchfabApiUrl}/me/collections", sketchFabToken, tokenType,
            SketchfabJsonContext.Default.PagedResultCollection, cancellationToken).ConfigureAwait(false))
        {
            collections.Add(collection);
        }
        return collections;
    }

    public Task AddModelToCollectionAsync(string collectionId, string sketchFabToken, TokenType tokenType, params string[] modelIds)
        => AddModelToCollectionAsync(collectionId, sketchFabToken, tokenType, (IEnumerable<string>)modelIds, CancellationToken.None);

    public async Task AddModelToCollectionAsync(string collectionId, string sketchFabToken, TokenType tokenType, IEnumerable<string> modelIds, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionId);
        ArgumentNullException.ThrowIfNull(modelIds);
        var ids = modelIds as IReadOnlyCollection<string> ?? [.. modelIds];
        if (ids.Count == 0) throw new ArgumentException("At least one model id is required.", nameof(modelIds));

        _logger.LogInformation("Add {Count} model(s) to collection {CollectionId}", ids.Count, collectionId);

        using var form = new MultipartFormDataContent();
        form.AddRange(ids, "models");
        using var request = CreateRequest(HttpMethod.Post, $"{SketchfabApiUrl}/collections/{collectionId}/models", sketchFabToken, tokenType);
        request.Content = form;

        await SendAsync(request, nameof(AddModelToCollectionAsync), cancellationToken).ConfigureAwait(false);
    }
}
