namespace Shiko.ExternalAuth.API.Models;

public sealed record UserInfoDto
(
    string UserId,
    string Email,
    IList<string> Roles
);