using TicTacToe.DataSource.Enum;

namespace TicTacToe.Web.Models;

public class CurrentGameDAO
{
    public GameBoardDAO GameBoard { get; set; } = new();
    public string Uuid { get; set; } = "";
    public string? Action { get; set; }
    public string PlayerX { get; set; } = "";
    public string? PlayerO { get; set; }
    public int State { get; set; } = (int)GameStates.WAIT;
    public bool IsPlayerX { get; set; } = true;
    public const int size = 3, playerX = 1, playerO = 2;
    public CurrentGameDAO()
    {
    }
    public CurrentGameDAO(int[,] field, string uuid)
    {
        GameBoard = new(field);
        Uuid = uuid;
    }
}