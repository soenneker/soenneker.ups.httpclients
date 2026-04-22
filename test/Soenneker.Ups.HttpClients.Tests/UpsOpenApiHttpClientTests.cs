using Soenneker.Ups.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Ups.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class UpsOpenApiHttpClientTests : HostedUnitTest
{
    private readonly IUpsOpenApiHttpClient _httpclient;

    public UpsOpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<IUpsOpenApiHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
