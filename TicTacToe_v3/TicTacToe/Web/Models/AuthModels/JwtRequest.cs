namespace TicTacToe.Web.Models;

public class JwtRequest
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IncorrectData { get; set; } = false;
}