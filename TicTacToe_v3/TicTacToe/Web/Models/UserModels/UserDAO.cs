namespace TicTacToe.Web.Models;

public class UserDAO
{
    public string Uuid { get; set; } = "";
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public bool PasswordCorrect = true;
}