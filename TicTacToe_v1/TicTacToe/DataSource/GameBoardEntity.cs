namespace TicTacToe.DataSource;

public class GameBoardEntity {
  public int[,] field = new int[3, 3];
  public GameBoardEntity() {}
  public GameBoardEntity(int[,] field) { this.field = field; }
}