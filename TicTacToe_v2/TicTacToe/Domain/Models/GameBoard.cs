namespace TicTacToe.Domain.Models;

public class GameBoard
{
    public int[,] Field;

    public GameBoard()
    {
        Field = new int[3, 3];
    }

    public GameBoard(int[,] field)
    {
        Field = field;
    }
}