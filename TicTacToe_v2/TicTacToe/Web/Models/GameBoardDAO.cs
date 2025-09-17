namespace TicTacToe.Web.Models;

public class GameBoardDAO
{
    public int[,] Field;
    public GameBoardDAO()
    {
        Field = new int[3, 3];
    }
    public GameBoardDAO(int[,] field)
    {
        Field = field;
    }
}