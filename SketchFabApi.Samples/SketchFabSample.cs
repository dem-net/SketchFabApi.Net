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
        private readonly SketchFabSampleOptions options;

        public SketchFabSample(ILogger<SketchFabSample> logger
            , IOptions<SketchFabSampleOptions> options
            , SketchFab.SketchFabApi sketchFabApi)
        {
            this.logger = logger;
            this.sketchFabApi = sketchFabApi;
            this.options = options.Value;
        }

        internal async Task Run()
        {
            try
            {
                // Test get my account
                await GetAccount(options.BearerToken, TokenType.Bearer);

                // Test add to collection
                await AddModelsToCollection(collectionId: "ef4914cce6a842589fecd78ac206a09b", options.BearerToken, TokenType.Bearer, modelIds: "f9d96fca765044f6a0e83f24bd9dcaa0");



            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: " + ex.Message);
            }
        }

        private async Task AddModelsToCollection(string collectionId, string bearerToken, TokenType bearer, params string[] modelIds)
        {
            try
            {
                logger.LogInformation($"Add models to collection");

                await sketchFabApi.AddModelToCollection(collectionId, bearerToken, bearer, modelIds);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: " + ex.Message);
            }
        }

        private async Task GetAccount(string token, TokenType tokenType)
        {
            try
            {
                logger.LogInformation($"Getting account info...");

                var account = await sketchFabApi.GetMyAccount(token, tokenType);

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
