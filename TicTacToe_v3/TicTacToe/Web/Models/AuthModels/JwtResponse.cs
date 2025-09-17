namespace TicTacToe.Web.Models;

public class JwtResponse
{
    public int Type { get; set; }
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";

}