using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Users.Api.Tests;

public class UsersEndpointsTests : IClassFixture<ApplicationFactory>
{
    private readonly ApplicationFactory _factory;

    public UsersEndpointsTests(ApplicationFactory factory) => _factory = factory;


    [Fact]
    public void GetUsersTests()
    {
        var client = _factory.CreateClient();

        var users = client.GetFromJsonAsync<IReadOnlyCollection<UserViewModel>>("/users").GetAwaiter().GetResult();

        users.Should().NotBeNull();
    }

    [Fact]
    public void GetUserByIdTests_GivenInvalidUserId_ShouldReturnNotFound()
    {
        var client = _factory.CreateClient();

        var response = client.GetAsync("/users/100").GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public void GetUserByIdTests()
    {
        var userId = 0;

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.Configure(app =>
            {
                using var scope = app.ApplicationServices.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                ApplicationUser user = new()
                {
                    FullName = "Test User",
                    Email = "test@example.com",
                    UserName = "test@example.com",
                    Type = UserType.Patient
                };

                userManager.CreateAsync(user).GetAwaiter().GetResult();

                userId = user.Id;
            });
        }).CreateClient();

        var url = $"/users/{userId}";

        var result = client.GetAsync(url).GetAwaiter().GetResult();

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var user = JsonConvert.DeserializeObject<UserViewModel>(responseContent);
        user.Should().NotBeNull();
        user!.FullName.Should().Be("Test User");
        user!.Email.Should().Be("test@example.com");
    }
}