using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TicTacToe.Web.Models;

public class AuthOptions
{
    public const string ISSUER = "TicTacToeServer";
    public const string AUDIENCE = "TicTacToeAuthClient";
    public const double ACCESS_LIFESPAN = 10;
    public const double REFRESH_LIFESPAN = 60 * 24 * 3;
    const string ACCESS_KEY = "caffeine-addled-horse-chewing-cables";
    const string REFRESH_KEY = "jumpy-rabbit-escaped-its-copper-cage";
    public static SymmetricSecurityKey GetSymmetricAccessSecurityKey()
    {
        return new(Encoding.UTF8.GetBytes(ACCESS_KEY));
    }
    public static SymmetricSecurityKey GetSymmetricRefreshSecurityKey()
    {
        return new(Encoding.UTF8.GetBytes(REFRESH_KEY));
    }
}