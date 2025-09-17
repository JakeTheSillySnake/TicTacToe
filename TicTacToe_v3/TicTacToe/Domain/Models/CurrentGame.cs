namespace TicTacToe.Domain.Models;

public class CurrentGame
{
    public GameBoard gameBoard = new();
    public string Uuid = "";
    public CurrentGame()
    {
        Uuid = Guid.NewGuid().ToString();
    }
    public CurrentGame(int[,] Field, string uuid)
    {
        gameBoard = new(Field);
        Uuid = uuid;
    }
}