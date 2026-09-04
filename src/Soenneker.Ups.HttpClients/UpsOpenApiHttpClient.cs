using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Ups.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Ups.HttpClients;

/// <inheritdoc cref="IUpsOpenApiHttpClient" />
public sealed class UpsOpenApiHttpClient : IUpsOpenApiHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;
    private readonly string _cacheKey = $"{nameof(UpsOpenApiHttpClient)}:{Guid.NewGuid():N}";

    private const string _prodBaseUrl = "https://onlinetools.ups.com/api";

    public UpsOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _config = config;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(_cacheKey, (config: _config, baseUrl: _config["Ups:ClientBaseUrl"] ?? _prodBaseUrl), static state =>
        {
            string accessToken = state.config["Ups:AccessToken"] ?? state.config.GetValueStrict<string>("Ups:ApiKey");
            string authHeaderName = state.config["Ups:AuthHeaderName"] ?? "Authorization";
            string authHeaderValueTemplate = state.config["Ups:AuthHeaderValueTemplate"] ?? "Bearer {token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", accessToken, StringComparison.Ordinal);

            return new HttpClientOptions
            {
                BaseAddress = new Uri(state.baseUrl),
                DefaultRequestHeaders = new Dictionary<string, string>
                {
                    {authHeaderName, authHeaderValue},
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        _httpClientCache.RemoveSync(_cacheKey);
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(_cacheKey);
    }
}
