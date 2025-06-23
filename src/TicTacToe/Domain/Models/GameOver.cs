namespace TicTacToe.Domain.Models;

public class GameOver
(bool s, int w)
{
    public bool status = s;
    public int winner = w;
}