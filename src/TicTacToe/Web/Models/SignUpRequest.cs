namespace TicTacToe.Web.Models;

public class SignUpRequest
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public bool loginExists = false;
}