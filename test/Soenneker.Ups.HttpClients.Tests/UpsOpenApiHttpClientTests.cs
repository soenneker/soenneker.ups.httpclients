using Soenneker.Ups.HttpClients.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Ups.HttpClients.Tests;

[Collection("Collection")]
public sealed class UpsOpenApiHttpClientTests : FixturedUnitTest
{
    private readonly IUpsOpenApiHttpClient _httpclient;

    public UpsOpenApiHttpClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _httpclient = Resolve<IUpsOpenApiHttpClient>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
