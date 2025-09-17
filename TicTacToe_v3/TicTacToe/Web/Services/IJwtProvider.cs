namespace TicTacToe.Web.Services;

using System.Security.Claims;
using TicTacToe.Web.Models;

public interface IJwtProvider
{
    public string? GetNewAccessToken(UserDAO user);
    public string? GetNewRefreshToken(UserDAO user);
    public ClaimsPrincipal? ValidateToken(string token, bool isAccessToken);
    public (string? name, string? uuid) GetClaims(HttpContext context);
    public (string? name, string? uuid) GetClaims(string token, bool isAccessToken);
}