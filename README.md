[![](https://img.shields.io/nuget/v/soenneker.ups.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.ups.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.ups.httpclients/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.ups.httpclients/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.ups.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.ups.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.ups.httpclients/codeql.yml?style=for-the-badge&label=CodeQL)](https://github.com/soenneker/soenneker.ups.httpclients/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Ups.HttpClients

Provides a cached `HttpClient` for UPS APIs using an already-issued OAuth access token.

## Installation

```bash
dotnet add package Soenneker.Ups.HttpClients
```

## Configuration

```json
{
  "Ups": {
    "AccessToken": "your-oauth-access-token"
  }
}
```

UPS client credentials are exchanged for an access token separately; this package does not perform or refresh that OAuth exchange. `Ups:ApiKey` remains supported as a legacy configuration name for the same token value.

The production base URL is `https://onlinetools.ups.com/api`. Set `Ups:ClientBaseUrl` when using a UPS test environment.

## Registration

```csharp
using Soenneker.Ups.HttpClients.Registrars;

services.AddUpsOpenApiHttpClientAsScoped();
```

Each scoped provider owns a separate cached client, so a new scope can read a rotated access token without disposing another scope's client. `AddUpsOpenApiHttpClientAsSingleton()` is also available, but it captures the token on first use and should only be used when that lifetime matches the token lifecycle.

## Usage

```csharp
using Soenneker.Ups.HttpClients.Abstract;

public sealed class UpsApiCaller
{
    private readonly IUpsOpenApiHttpClient _clients;

    public UpsApiCaller(IUpsOpenApiHttpClient clients)
    {
        _clients = clients;
    }

    public async ValueTask<HttpResponseMessage> Send(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        HttpClient client = await _clients.Get(cancellationToken);
        return await client.SendAsync(request, cancellationToken);
    }
}
```

Requests include `Authorization: Bearer <AccessToken>` by default. Disposal removes only the client owned by that provider instance.
