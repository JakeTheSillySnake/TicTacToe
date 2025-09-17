namespace TicTacToe.Web.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.Web.Models;

public class JwtProvider : IJwtProvider
{
    private readonly string uuidClaim = "UserIdPrincipal";
    public string? GetNewAccessToken(UserDAO user)
    {
        return BuildToken(user, true);
    }
    public string? GetNewRefreshToken(UserDAO user)
    {
        return BuildToken(user, false);
    }
    public ClaimsPrincipal? ValidateToken(string token, bool isAccessToken)
    {
        IdentityModelEventSource.ShowPII = true;
        var key = isAccessToken ? AuthOptions.GetSymmetricAccessSecurityKey() : AuthOptions.GetSymmetricRefreshSecurityKey();

        var tokenParams = new TokenValidationParameters
        {
            ValidAudience = AuthOptions.AUDIENCE,
            ValidIssuer = AuthOptions.ISSUER,
            IssuerSigningKey = key,
            ValidateLifetime = true
        };

        try
        {
            return new JwtSecurityTokenHandler().ValidateToken(token, tokenParams, out _);
        }
        catch
        {
            return null;
        }
    }
    public (string? name, string? uuid) GetClaims(HttpContext context)
    {
        string? name = context.User?.Identity?.Name;
        string? uuid = context.User?.FindFirst(uuidClaim)?.Value;

        return (name, uuid);
    }
    public (string? name, string? uuid) GetClaims(string token, bool isAccessToken)
    {
        var principal = ValidateToken(token, isAccessToken);

        string? name = principal?.Identity?.Name;
        string? uuid = principal?.FindFirst(uuidClaim)?.Value;

        return (name, uuid);
    }
    private string? BuildToken(UserDAO user, bool isAccessToken)
    {
        var claims = new List<Claim> { new(ClaimTypes.Name, user.Login), new(uuidClaim, user.Uuid) };
        var timeSpan = isAccessToken ? AuthOptions.ACCESS_LIFESPAN : AuthOptions.REFRESH_LIFESPAN;
        var key = isAccessToken ? AuthOptions.GetSymmetricAccessSecurityKey() : AuthOptions.GetSymmetricRefreshSecurityKey();

        var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(timeSpan)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}