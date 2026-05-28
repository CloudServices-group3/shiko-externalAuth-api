namespace Shiko.ExternalAuth.API.Models;

/// <summary>
/// Request record for google login, 
/// contains the google id token received from the client, 
/// -> validated against google before sent to auth API
/// </summary>
public record GoogleTokenRequest(string IdToken);