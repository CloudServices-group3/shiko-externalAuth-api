using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using Shiko.ExternalAuth.API.Models;
using Shiko.ExternalAuth.API.Services;

namespace Shiko.ExternalAuth.API.Endpoints;

public static class ExternalAuthEndpoints
{
    public static void MapExternalAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/external-auth");

        group.MapPost("/google", GoogleLogin)
            .WithSummary("Google Login")
            .WithDescription("Login with Google by providing a valid Google ID token." +
            " Returns access and refresh tokens if successful.")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized); 

    }

    private static async Task<IResult> GoogleLogin(
        GoogleTokenRequest request,
        GoogleAuthService googleAuthService
        )
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

            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();

            return Results.Ok(loginResult);
        }
        catch (InvalidJwtException)
        {
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            // catch all errors and log for debugging
            Console.WriteLine($"[CRITICAL ERROR] google login failed: {ex.Message}");
            return Results.Problem("something went wrong trying to log in.", statusCode: 500);
        }
    }
}
