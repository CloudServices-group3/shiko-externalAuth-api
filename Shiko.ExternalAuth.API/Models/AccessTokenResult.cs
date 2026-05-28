namespace Shiko.ExternalAuth.API.Models;

public sealed record AccessTokenResult
(
    string AccessToken,
    string TokenType,
    int Expires,       
    DateTimeOffset ExpiresAtUtc
);
