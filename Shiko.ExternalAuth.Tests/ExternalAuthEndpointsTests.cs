
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Shiko.ExternalAuth.API.Models;
using Shiko.ExternalAuth.API.Services;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;



namespace Shiko.ExternalAuth.Tests;

// IClassFixture running Program.cs in memory -> real HTTP requests to API endpoints without starting the whole app on a port
public class ExternalAuthEndpointsTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
   
    [Fact]
    public async Task GoogleLogin_Endpoint_ShouldReturnOk_AndDeserializeCorrectly()
    {
        // ARRANGE
        // create mock of GoogleAuthService and pass in dummy config and factory as parameters
        var dummyConfig = Substitute.For<IConfiguration>();
        var dummyFactory = Substitute.For<IHttpClientFactory>();
        var mockAuthService = Substitute.For<IGoogleAuthService>();

        // create expected results based on the dtos/records
        var expectedUserInfo = new UserInfoDto("user-123", "student@shiko.se", new List<string> { "Student" });
        var expectedTokenResult = new AccessTokenResult("fake-shiko-access-jwt", "Bearer", 3600, DateTimeOffset.UtcNow.AddHours(1));
        var expectedLoginResult = new LoginResult(expectedTokenResult, "fake-refresh-token", expectedUserInfo);

        // fake HttpResponseMessage as result to API
        var fakeHttpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedLoginResult)
        };

        // tell mocked service to return the fake response when called 
        mockAuthService.ExternalLoginAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(fakeHttpResponse);

        // fake google payload 
        var fakeGooglePayload = new Google.Apis.Auth.GoogleJsonWebSignature.Payload
        {
            Email = "student@shiko.se",
            Name = "Test Student"
        };
        mockAuthService.ValidateGoogleTokenAsync(Arg.Any<string>()).Returns(fakeGooglePayload);

        // start application in memory with mocked GoogleAuthService
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IGoogleAuthService));
                if (descriptor != null) services.Remove(descriptor);

                services.AddScoped<IGoogleAuthService>(_ => mockAuthService);
            });
        }).CreateClient();

        var requestBody = new GoogleTokenRequest("mocked-google-token");

        // ACT
        // Http request to the actual API endpoint, which will use the mocked GoogleAuthService and return our fakeHttpResponse
        var response = await client.PostAsJsonAsync("/api/external-auth/google", requestBody);

        // ASSERT
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // check that json is deserialized correctly and converted to LoginResult object,
       
        var finalResult = await response.Content.ReadFromJsonAsync<LoginResult>();

        Assert.NotNull(finalResult);
        Assert.Equal("fake-shiko-access-jwt", finalResult.AccessToken.AccessToken);
        Assert.Equal("student@shiko.se", finalResult.UserInfo.Email);
    }
}



