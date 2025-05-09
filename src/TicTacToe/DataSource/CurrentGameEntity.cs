using System.Drawing;

namespace TicTacToe.Data;

public class CurrentGameEntity {
  public GameBoardEntity gameBoard = new();
  public string uuid = "";
  public static int size = 3, player = 1, opponent = 2;
  public CurrentGameEntity() { uuid = Guid.NewGuid().ToString(); }
  public CurrentGameEntity(int[,] field, string uuid) {
    gameBoard = new(field);
    this.uuid = uuid;
  }
}