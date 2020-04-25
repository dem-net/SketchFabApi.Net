//
// SketchFabSample.cs
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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SketchFab;

namespace SketchFabApi.Samples
{
    public class SketchFabSample
    {
        private readonly ILogger<SketchFabSample> logger;
        private readonly SketchFab.SketchFabApi sketchFabApi;
        private readonly AppSecrets secrets;

        public SketchFabSample(ILogger<SketchFabSample> logger
            , IOptions<AppSecrets> secrets
            , SketchFab.SketchFabApi sketchFabApi)
        {
            this.logger = logger;
            this.sketchFabApi = sketchFabApi;
            this.secrets = secrets.Value;
        }

        internal async Task Run()
        {
            try
            {

                HashSet<string> allowedTags = new HashSet<string>(new string[] { "mycenaean-atlas", "elevationapi" });
                int numModelsUpdated = 0;
                int numModelsSkipped = 0;
                await foreach(var myModel in sketchFabApi.GetMyModelsAsync(secrets.SketchFabToken, TokenType.Token))
                {
                    // If model has categories or forbidden tags, update it
                    if (myModel.categories.Any() || myModel.tags.Any(t => !allowedTags.Contains(t.name)))
                    {
                        await Task.Delay(5000); // avoid HTTP 429 too many requests

                        UpdateModelRequest uploadRequest = new UpdateModelRequest()
                        {
                            Categories = new List<string>(),
                            Tags = allowedTags.ToList(),
                            ModelId = myModel.uid,
                        };
                        await sketchFabApi.UpdateModelAsync(myModel.uid, uploadRequest, secrets.SketchFabToken, TokenType.Token);
                        numModelsUpdated++;
                    }
                    else
                    {
                        numModelsSkipped++;
                    }


                    logger.LogInformation($"Update models progress: {numModelsUpdated} updated, {numModelsSkipped} skipped");
                }

                // Test get model
                var model = await sketchFabApi.GetModelAsync("f934cb5207624d14841f19ca85b4ea2c");

                // Test get my account
                await GetAccountAsync(secrets.SketchFabToken, TokenType.Bearer);

                // Test add to collection
                await AddModelsToCollectionAsync(collectionId: "ef4914cce6a842589fecd78ac206a09b", secrets.SketchFabToken, TokenType.Bearer, modelIds: "f9d96fca765044f6a0e83f24bd9dcaa0");



            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: " + ex.Message);
            }
        }

        private async Task AddModelsToCollectionAsync(string collectionId, string bearerToken, TokenType bearer, params string[] modelIds)
        {
            try
            {
                logger.LogInformation($"Add models to collection");

                await sketchFabApi.AddModelToCollectionAsync(collectionId, bearerToken, bearer, modelIds);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: " + ex.Message);
            }
        }

        private async Task GetAccountAsync(string token, TokenType tokenType)
        {
            try
            {
                logger.LogInformation($"Getting account info...");

                var account = await sketchFabApi.GetMyAccountAsync(token, tokenType);

                var uploadLimit = account.uploadSizeLimit;
                logger.LogInformation($"Upload size limit for {account.account}: {uploadLimit}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: " + ex.Message);
            }
        }
    }
}
