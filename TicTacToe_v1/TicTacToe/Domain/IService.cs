namespace TicTacToe.Domain;

public interface IService {
  public GameBoard NextMove();
  public bool IsValid(int row, int col);
  public GameOver IsOver();
}