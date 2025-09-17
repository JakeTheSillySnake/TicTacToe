namespace TicTacToe.DataSource.Models;

public class UserWinRatioDTO
{
    public string Uuid { get; set; } = "";
    public int Wins { get; set; }
    public int Total { get; set; }
    public float WinRatio { get; set; }
}