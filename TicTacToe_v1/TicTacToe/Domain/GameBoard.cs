namespace TicTacToe.Domain;

public class GameBoard {
  public int[,] field = new int[3, 3];
  public GameBoard() {}
  public GameBoard(int[,] field) { this.field = field; }
}