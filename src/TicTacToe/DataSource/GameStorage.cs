using System.Collections.Concurrent;

namespace TicTacToe.Data;

public class GameStorage {
  public ConcurrentDictionary<string, CurrentGameEntity> games = [];
}