using System.Collections.Concurrent;

namespace TicTacToe.DataSource;

public class GameStorage {
  private static GameStorage? instance;
  public ConcurrentDictionary<string, CurrentGameEntity> games = [];
  private GameStorage() {}
  public static GameStorage GetInstance() {
    if (instance == null)
      instance = new GameStorage();
    return instance;
  }
}