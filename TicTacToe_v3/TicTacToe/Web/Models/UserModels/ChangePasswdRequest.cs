namespace TicTacToe.Web.Models;

public class ChangePasswdRequest
{
    public string NewPassword { get; set; } = "";
    public string CurrentPassword { get; set; } = "";
}