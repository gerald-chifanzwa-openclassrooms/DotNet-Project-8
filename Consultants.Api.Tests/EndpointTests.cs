using System.Collections.Generic;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Consultants.Api.Tests;

public class EndpointTests : IClassFixture<ApplicationFactory>
{
    private readonly ApplicationFactory _factory;

    public EndpointTests(ApplicationFactory factory) => _factory = factory;

    [Fact]
    public void GetConsultantsTest_ShouldReturnSavedConsultants()
    {
        var httpClient = _factory.CreateClient();

        var results = httpClient.GetFromJsonAsync<IReadOnlyList<ConsultantViewModel>>("/consultants").GetAwaiter().GetResult();

        results.Should().NotBeEmpty();
        results.Should().HaveCount(4);
    }

    [Fact]
    public void GetConsultantsByIdTest_GivenValidConsultantId_ShouldReturnSavedConsultant()
    {
        var httpClient = _factory.CreateClient();

        var results = httpClient.GetFromJsonAsync<ConsultantViewModel>("/consultants/1").GetAwaiter().GetResult();

        results.Should().NotBeNull();
    }

    [Fact]
    public void GetConsultantsByIdTest_GivenInValidConsultantId_ShouldReturnNotFound()
    {
        var httpClient = _factory.CreateClient();

        var response = httpClient.GetAsync("/consultants/90").GetAwaiter().GetResult();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
