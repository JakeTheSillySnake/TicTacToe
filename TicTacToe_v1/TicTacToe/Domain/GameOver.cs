namespace TicTacToe.Domain;

public class GameOver {
  public bool status = false;
  public int winner = 0;
  public GameOver(bool s, int w) {
    status = s;
    winner = w;
  }
}