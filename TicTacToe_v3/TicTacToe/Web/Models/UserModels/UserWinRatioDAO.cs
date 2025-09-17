namespace TicTacToe.Web.Models;

public class UserWinRatioDAO
{
    public string Uuid { get; set; } = "";
    public string Name { get; set; } = "";
    public int Wins { get; set; }
    public int Total { get; set; }
    public float WinRatio { get; set; }
}