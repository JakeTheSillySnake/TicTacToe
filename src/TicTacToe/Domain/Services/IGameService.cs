namespace TicTacToe.Domain.Services;

using TicTacToe.Domain.Models;

public interface IGameService
{
    public GameBoard NextMove(CurrentGame game);
    public bool IsValid(CurrentGame game, int row, int col);
    public GameOver IsOver(CurrentGame game);
    public void MovePlayer(CurrentGame game, int row, int col, bool isPlayerX);
}