using Google.Apis.Auth;

namespace Shiko.ExternalAuth.API.Services;

public class GoogleAuthService(IConfiguration config, IHttpClientFactory httpClientFactory) : IGoogleAuthService
{
    public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [config["Google:ClientId"]]
        };

        return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
    }

    public async Task<HttpResponseMessage> ExternalLoginAsync(string email, string displayName)
    {
        var client = httpClientFactory.CreateClient("AuthApi");

        return await client.PostAsJsonAsync("/api/auth/external-login", new
        {
            Email = email,
            Provider = "Google"
        });
    }
}