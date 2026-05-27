using Google.Apis.Auth;
using Shiko.ExternalAuth.API.Models;
using Shiko.ExternalAuth.API.Services;

namespace Shiko.ExternalAuth.API.Endpoints;

public static class ExternalAuthEndpoints
{
    public static void MapExternalAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/external-auth");

        group.MapPost("/google", GoogleLogin);
    }

    private static async Task<IResult> GoogleLogin(
        GoogleTokenRequest request,
        GoogleAuthService googleAuthService)
    {
        try
        {
            // validate token and get user info (payload) from Google
            var payload = await googleAuthService.ValidateGoogleTokenAsync(request.IdToken);

            // send user email and name to auth api
            var response = await googleAuthService.ExternalLoginAsync(
                payload.Email,
                payload.Name
            );

            if (!response.IsSuccessStatusCode)
                return Results.Unauthorized();

            // return access + refresh token
            var result = await response.Content.ReadFromJsonAsync<object>();
            return Results.Ok(result);
        }
        catch (InvalidJwtException)
        {
            return Results.Unauthorized();
        }
    }
}
