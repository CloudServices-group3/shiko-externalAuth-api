using Google.Apis.Auth;

namespace Shiko.ExternalAuth.API.Services;

public interface IGoogleAuthService
{
    Task<HttpResponseMessage> ExternalLoginAsync(string email, string displayName);
    Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);
}
