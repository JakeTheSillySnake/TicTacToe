namespace TicTacToe.Web;

public class CurrentGameWebEntity {
  public GameBoardWebEntity gameBoard { get; set; } = new();
  public string uuid { get; set; } = "";
  public string? action { get; set; }
  public int winner = 0;
  public bool isOver = false;
  public const int size = 3, player = 1, opponent = 2;
  public CurrentGameWebEntity() { uuid = Guid.NewGuid().ToString(); }
  public CurrentGameWebEntity(int[,] field, string uuid, bool isOver,
                              int winner) {
    gameBoard = new(field);
    this.uuid = uuid;
    this.isOver = isOver;
    this.winner = winner;
  }
}