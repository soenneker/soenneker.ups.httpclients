using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Ups.HttpClients.Abstract;

/// <summary>
/// Provides an HTTP client authenticated with a UPS OAuth access token.
/// </summary>
public interface IUpsOpenApiHttpClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the cached HTTP client, creating it on first use.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel client creation.</param>
    /// <returns>The authenticated HTTP client.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
