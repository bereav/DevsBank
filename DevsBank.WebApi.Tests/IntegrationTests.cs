using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;

namespace DevsBank.WebApi.Tests;

public class IntegrationTests
{
    private readonly HttpClient _httpClient;
    public IntegrationTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Startup>();
        _httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task When_opening_an_account_it_should_become_available()
    {
        // Arrange
        var customerId = Guid.Parse("4886d666-32e6-4deb-b6e9-947f3334ca84"); // an existing user.
        var credit = 1000;

        // Act
        var response = await _httpClient.PostAsync($"/api/v1.0/Accounts?customerId={customerId}&credit={credit}", null);
        response.EnsureSuccessStatusCode();

        var newAccountId = JsonConvert.DeserializeObject<Guid>(await response.Content.ReadAsStringAsync());

        // Assert
        newAccountId.Should().NotBe(Guid.Empty);
    }
}