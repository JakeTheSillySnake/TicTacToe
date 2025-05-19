namespace TicTacToe.Web;

public class GameBoardWebEntity {
  public int[,] field = new int[3, 3];
  public GameBoardWebEntity() {}
  public GameBoardWebEntity(int[,] field) { this.field = field; }
}