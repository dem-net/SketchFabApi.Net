using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Sketchfab;
using Xunit;

namespace SketchFabApi.Net.Tests;

/// <summary>
/// Deserialization of real Sketchfab responses (captured from the public API) and HTTP behavior against a fake handler.
/// </summary>
public class SketchfabApiTests
{
    private static string Fixture(string name) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", name));

    [Fact]
    public void Model_DeserializesRealResponse()
    {
        var model = JsonSerializer.Deserialize(Fixture("model.json"), SketchfabJsonContext.Default.Model)!;

        Assert.Equal("f934cb5207624d14841f19ca85b4ea2c", model.uid);
        Assert.True(model.IsReady());
        Assert.Equal("xfischer", model.user.username);
        Assert.Equal("by-nc-sa", model.license.slug);
        Assert.Equal(7, model.tags.Count);
        Assert.Equal(5, model.thumbnails.images.Count);
        Assert.Equal(1920, model.thumbnails.images[0].width);
        Assert.Equal(new DateTime(2020, 4, 21, 23, 20, 34), model.createdAt.AddTicks(-(model.createdAt.Ticks % TimeSpan.TicksPerSecond)));
        Assert.NotNull(model.publishedAt);
    }

    [Fact]
    public void PagedResults_DeserializeRealResponses()
    {
        var models = JsonSerializer.Deserialize(Fixture("models-page.json"), SketchfabJsonContext.Default.PagedResultModel)!;
        Assert.Equal(2, models.results.Count);
        Assert.NotNull(models.next);

        var collections = JsonSerializer.Deserialize(Fixture("collections-page.json"), SketchfabJsonContext.Default.PagedResultCollection)!;
        Assert.Equal(2, collections.results.Count);
        Assert.Equal("ef4914cce6a842589fecd78ac206a09b", collections.results[0].uid);
        Assert.NotNull(collections.next);
    }

    [Fact]
    public void ModelOptions_SerializeLikeTheNewtonsoftSettings()
    {
        // camelCase, enum names, nulls omitted (same payload as the former global Newtonsoft settings)
        var json = JsonSerializer.Serialize(new ModelOptions(), SketchfabJsonContext.Default.ModelOptions);

        Assert.Equal("""{"shading":"lit","background":{"environment":"df380da788ee444885722735039b0c09"}}""", json);
    }

    [Fact]
    public async Task GetMyModels_FollowsNextLinks_WithAuthOnEveryPage()
    {
        var handler = new FakeHandler(req => req.RequestUri!.AbsoluteUri switch
        {
            "https://api.sketchfab.com/v3/me/models" => Json(Page("https://api.sketchfab.com/v3/me/models?cursor=2", "a", "b")),
            "https://api.sketchfab.com/v3/me/models?cursor=2" => Json(Page(null, "c")),
            var other => throw new InvalidOperationException($"Unexpected request {other}"),
        });
        var api = CreateApi(handler);

        var uids = new List<string>();
        await foreach (var model in api.GetMyModelsAsync("secret", TokenType.Bearer))
        {
            uids.Add(model.uid);
        }

        Assert.Equal(["a", "b", "c"], uids);
        Assert.Equal(2, handler.Requests.Count);
        Assert.All(handler.Requests, r => Assert.Equal("Bearer secret", r.Authorization));
    }

    [Fact]
    public async Task GetMyModels_FailsOnAnErrorPage()
    {
        var handler = new FakeHandler(req => req.RequestUri!.Query == ""
            ? Json(Page("https://api.sketchfab.com/v3/me/models?cursor=2", "a"))
            : new HttpResponseMessage(HttpStatusCode.TooManyRequests) { Content = new StringContent("""{"detail":"slow down"}""") });
        var api = CreateApi(handler);

        var ex = await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await foreach (var _ in api.GetMyModelsAsync("secret", TokenType.Token)) { }
        });
        Assert.Equal(HttpStatusCode.TooManyRequests, ex.StatusCode);
    }

    [Fact]
    public async Task GetMyModels_HonorsCancellation()
    {
        using var cts = new CancellationTokenSource();
        var handler = new FakeHandler(_ => Json(Page("https://api.sketchfab.com/v3/me/models?cursor=2", "a")));
        var api = CreateApi(handler);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await foreach (var _ in api.GetMyModelsAsync("secret", TokenType.Token, cts.Token))
            {
                cts.Cancel();
            }
        });
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task UploadModel_StreamsTheFileAsOctetStream_AndReadsTheLocationHeader()
    {
        var file = Path.Combine(Path.GetTempPath(), $"sketchfab-test-{Guid.NewGuid():N}.glb");
        await File.WriteAllBytesAsync(file, Encoding.ASCII.GetBytes("glTF-test-payload"));
        try
        {
            var handler = new FakeHandler(_ =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.Created) { Content = new StringContent("{}") };
                response.Headers.TryAddWithoutValidation("Location", "new-model-uid");
                return response;
            });
            var api = CreateApi(handler);
            var request = new UploadModelRequest { FilePath = file, Name = "Test", Description = "d", Source = "tests", TokenType = TokenType.Bearer };

            var result = await api.UploadModelAsync(request, "secret");

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("new-model-uid", result.ModelId);
            Assert.Equal("new-model-uid", request.ModelId);

            var sent = Assert.Single(handler.Requests);
            Assert.Equal("Bearer secret", sent.Authorization);
            var filePart = Assert.Single(sent.Parts, p => p.Name == "modelFile");
            Assert.Equal("application/octet-stream", filePart.ContentType);
            Assert.Equal(Path.GetFileName(file), filePart.FileName);
            Assert.Equal("glTF-test-payload", filePart.Body);
            Assert.Contains(sent.Parts, p => p.Name == "options" && p.Body.Contains("\"shading\":\"lit\""));
        }
        finally
        {
            File.Delete(file);
        }
    }

    [Fact]
    public async Task UploadModel_ReturnsTheErrorStatus_WithoutThrowing()
    {
        var file = Path.GetTempFileName();
        try
        {
            var api = CreateApi(new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("""{"detail":"bad file"}""") }));

            var result = await api.UploadModelAsync(new UploadModelRequest { FilePath = file, Name = "n", Description = "d" }, "secret");

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Null(result.ModelId);
        }
        finally
        {
            File.Delete(file);
        }
    }

    private static SketchfabApi CreateApi(FakeHandler handler)
    {
        var services = new ServiceCollection().AddLogging();
        services.AddSketchfabApi().ConfigurePrimaryHttpMessageHandler(() => handler);
        return services.BuildServiceProvider().GetRequiredService<SketchfabApi>();
    }

    private static string Page(string? next, params string[] uids)
    {
        var results = string.Join(",", uids.Select(uid => $$$"""{"uid":"{{{uid}}}","status":{"processing":"SUCCEEDED"}}"""));
        var nextJson = next is null ? "null" : $"\"{next}\"";
        return $$"""{"next":{{nextJson}},"previous":null,"results":[{{results}}]}""";
    }

    private static HttpResponseMessage Json(string json) => new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    private sealed record SentPart(string? Name, string? FileName, string? ContentType, string Body);

    private sealed record SentRequest(Uri? RequestUri, string? Authorization, List<SentPart> Parts);

    /// <summary>
    /// Records each request (headers and multipart parts, read before the client disposes them) and answers with the callback.
    /// </summary>
    private sealed class FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        public List<SentRequest> Requests { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var parts = new List<SentPart>();
            if (request.Content is MultipartFormDataContent form)
            {
                foreach (var part in form)
                {
                    parts.Add(new SentPart(part.Headers.ContentDisposition?.Name?.Trim('"'), part.Headers.ContentDisposition?.FileName?.Trim('"'),
                        part.Headers.ContentType?.MediaType, await part.ReadAsStringAsync(cancellationToken)));
                }
            }
            Requests.Add(new SentRequest(request.RequestUri, request.Headers.Authorization?.ToString(), parts));
            return respond(request);
        }
    }
}
