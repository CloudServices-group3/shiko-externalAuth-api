namespace Shiko.ExternalAuth.API.Models;

public record ExternalLoginRequest(
    string Email,
    string Provider
    );