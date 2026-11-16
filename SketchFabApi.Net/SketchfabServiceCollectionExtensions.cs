using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sketchfab;

namespace Microsoft.Extensions.DependencyInjection;

public static class SketchfabServiceCollectionExtensions
{
    /// <summary>
    /// Model uploads can be large: the default 100 s <see cref="System.Net.Http.HttpClient.Timeout"/> is too short for them.
    /// </summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Registers <see cref="SketchfabApi"/> and its named <see cref="System.Net.Http.HttpClient"/>.
    /// Use the returned builder to customize the client (handlers, timeout...).
    /// </summary>
    public static IHttpClientBuilder AddSketchfabApi(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddTransient<SketchfabApi>();
        return services.AddHttpClient(SketchfabApi.HttpClientName, client =>
        {
            client.Timeout = DefaultTimeout;
            var version = typeof(SketchfabApi).Assembly.GetName().Version?.ToString(3) ?? "0.0.0";
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("SketchfabApi.Net", version));
        });
    }
}
