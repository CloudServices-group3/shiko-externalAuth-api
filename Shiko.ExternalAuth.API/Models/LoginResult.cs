namespace Shiko.ExternalAuth.API.Models;

public sealed record LoginResult
(
    AccessTokenResult AccessToken,
    string RefreshToken,
    UserInfoDto UserInfo
);
